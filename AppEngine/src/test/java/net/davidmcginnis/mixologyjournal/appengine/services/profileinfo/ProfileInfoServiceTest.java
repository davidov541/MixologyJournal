package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.*;
import org.junit.jupiter.api.*;

import com.google.appengine.tools.development.testing.LocalDatastoreServiceTestConfig;
import com.google.appengine.tools.development.testing.LocalServiceTestHelper;

import static com.google.appengine.api.datastore.FetchOptions.Builder.withLimit;
import static org.junit.jupiter.api.Assertions.*;

class ProfileInfoServiceTest {

    private LocalServiceTestHelper helper;

    @BeforeEach
    void setUp() {
        helper = new LocalServiceTestHelper(new LocalDatastoreServiceTestConfig());
        helper.setUp();
    }

    @AfterEach
    void tearDown() {
        helper.tearDown();
    }


    @Test
    void testEmptyQuery() {
        String profileID = "TestProfileID";

        ProfileInfoService service = new ProfileInfoService();
        FullProfileResponse response = service.getUserInfo(new FullProfileRequest(profileID));
        assertEquals("Failure", response.getResult());
        assertNull(response.getProfile());
    }

    @Test
    void testSimpleQuery() {
        DatastoreService ds = DatastoreServiceFactory.getDatastoreService();
        String profileID = "TestProfileID";
        Entity e = new Entity(Profile.datastoreKindName, profileID);
        e.setProperty(Profile.datastoreProfileIDName, profileID);
        ds.put(e);

        ProfileInfoService service = new ProfileInfoService();
        FullProfileResponse response = service.getUserInfo(new FullProfileRequest(profileID));

        assertEquals("Success", response.getResult());
        assertTrue(response.getProfile().equals(e));
    }

    @Test
    void testSimpleInsert() {
        DatastoreService ds = DatastoreServiceFactory.getDatastoreService();
        ProfileInfoService service = new ProfileInfoService();
        String profileID = "TestProfileID";

        service.addUser(new FullProfileRequest(profileID));

        assertEquals(1, ds.prepare(new Query(Profile.datastoreKindName)).countEntities(withLimit(10)));
        PreparedQuery query = ds.prepare(new Query(Profile.datastoreKindName));
        assertEquals(1, query.countEntities(withLimit(10)));
        Entity entity = query.asList(withLimit(10)).get(0);

        assertEquals(profileID, entity.getProperty(Profile.datastoreProfileIDName).toString());
        assertEquals(profileID, entity.getKey().getName());
    }

    @Test
    void testUpdateWithNoChanges() {
        String profileID = "TestProfileID";

        Recipe[] drinks = new Recipe[] { };

        runUpdateTest(profileID, drinks);
    }

    @Test
    void testAddEmptyRecipe() {
        String profileID = "TestProfileID";
        String recipeID = "TestRecipeID";

        Entity profileEntity = new Entity(Profile.datastoreKindName, profileID);
        Drink[] drinks = new Drink[] { };
        Recipe[] recipes = new Recipe[] { Recipe.create(recipeID, profileEntity.getKey(), drinks) };

        runUpdateTest(profileID, recipes);
    }

    @Test
    void testAddRecipeAndDrink() {
        String profileID = "TestProfileID";
        String drinkID = "TestDrinkID";
        String recipeID = "TestRecipeID";

        Entity profileEntity = new Entity(Profile.datastoreKindName, profileID);
        Entity recipeEntity = new Entity(Recipe.kindName, recipeID, profileEntity.getKey());
        Drink[] drinks = new Drink[] { Drink.create(drinkID, recipeEntity.getKey())};
        Recipe[] recipes = new Recipe[] { Recipe.create(recipeID, profileEntity.getKey(), drinks) };

        runUpdateTest(profileID, recipes);
    }

    private void runUpdateTest(String profileID, Recipe[] recipes) {
        DatastoreService ds = DatastoreServiceFactory.getDatastoreService();
        ProfileInfoService service = new ProfileInfoService();
        Profile newProfile = Profile.get(profileID, recipes);

        service.addUser(new FullProfileRequest(profileID));

        service.syncUserInfo(new SyncProfileRequest(newProfile));

        assertEquals(1, ds.prepare(new Query(Profile.datastoreKindName)).countEntities(withLimit(10)));
        PreparedQuery profileQuery = ds.prepare(new Query(Profile.datastoreKindName));
        assertEquals(1, profileQuery.countEntities(withLimit(10)));

        Entity profileResultEntity = profileQuery.asList(withLimit(10)).get(0);
        assertTrue(newProfile.equals(profileResultEntity));

        PreparedQuery recipeQuery = ds.prepare(new Query(Recipe.kindName).setAncestor(profileResultEntity.getKey()));
        assertEquals(recipes.length, recipeQuery.countEntities(withLimit(10)));
        for(int i = 0; i < recipes.length; i++) {
            Entity recipeResultEntity = recipeQuery.asList(withLimit(10)).get(i);
            assertTrue(recipes[i].equals(recipeResultEntity));

            PreparedQuery drinkQuery = ds.prepare(new Query(Drink.kindName).setAncestor(recipeResultEntity.getKey()));
            Drink[] drinks = recipes[i].getDrinks();
            assertEquals(drinks.length, drinkQuery.countEntities(withLimit(10)));
            for(int j = 0; j < drinks.length; j++) {
                Entity drinkResultEntity = drinkQuery.asList(withLimit(10)).get(j);
                assertTrue(drinks[j].equals(drinkResultEntity));
            }
        }
    }
}