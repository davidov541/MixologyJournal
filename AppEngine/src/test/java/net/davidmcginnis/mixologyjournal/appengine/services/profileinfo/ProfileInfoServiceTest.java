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
        assertEquals(profileID, response.getProfile().getProfileID());
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

        Drink[] drinks = new Drink[] { };

        runUpdateTest(profileID, drinks);
    }

    @Test
    void testAddDrink() {
        String profileID = "TestProfileID";
        String drinkID = "TestDrinkID";
        String recipeID = "TestRecipeID";

        Entity profileEntity = new Entity(Profile.datastoreKindName, profileID);
        Recipe derivedRecipe = Recipe.createRecipe(recipeID);
        Drink[] drinks = new Drink[] { Drink.createDrink(drinkID, profileEntity.getKey(), derivedRecipe)};

        runUpdateTest(profileID, drinks);
    }

    private void runUpdateTest(String profileID, Drink[] drinks) {
        DatastoreService ds = DatastoreServiceFactory.getDatastoreService();
        ProfileInfoService service = new ProfileInfoService();
        Profile newProfile = Profile.createProfile(profileID, drinks);

        service.addUser(new FullProfileRequest(profileID));

        service.syncUserInfo(new SyncProfileRequest(newProfile));

        assertEquals(1, ds.prepare(new Query(Profile.datastoreKindName)).countEntities(withLimit(10)));
        PreparedQuery profileQuery = ds.prepare(new Query(Profile.datastoreKindName));
        assertEquals(1, profileQuery.countEntities(withLimit(10)));
        Entity profileResultEntity = profileQuery.asList(withLimit(10)).get(0);
        assertEquals(profileID, profileResultEntity.getProperty(Profile.datastoreProfileIDName).toString());
        assertEquals(profileID, profileResultEntity.getKey().getName());

        PreparedQuery drinkQuery = ds.prepare(new Query(Drink.datastoreKindName));
        assertEquals(drinks.length, drinkQuery.countEntities(withLimit(10)));
        for(int i = 0; i < drinks.length; i++) {
            Entity drinkResultEntity = drinkQuery.asList(withLimit(10)).get(i);
            assertEquals(drinks[i].getDrinkID(), drinkResultEntity.getProperty(Drink.datastoreDrinkIDName).toString());
            assertEquals(drinks[i].getDrinkID(), drinkResultEntity.getKey().getName());
            assertEquals(drinks[i].getDerivedRecipeKey(), drinkResultEntity.getProperty(Drink.datastoreDerivedRecipeName));
        }
    }
}