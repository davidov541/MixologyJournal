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
    public void setUp() {
        helper = new LocalServiceTestHelper(new LocalDatastoreServiceTestConfig());
        helper.setUp();
    }

    @AfterEach
    public void tearDown() {
        helper.tearDown();
    }

    @Test
    public void testUpdateWithNoChanges() throws EntityNotFoundException {
        DatastoreService ds = DatastoreServiceFactory.getDatastoreService();
        ProfileInfoService service = new ProfileInfoService();
        String profileID = "TestProfileID";
        service.addUser(new FullProfileRequest(profileID));

        Entity newEntity = new Entity(Profile.datastoreKindName, profileID);
        Profile newProfile = Profile.createProfile(ds, newEntity.getKey());
        service.syncUserInfo(new SyncProfileRequest(newProfile));

        assertEquals(1, ds.prepare(new Query(Profile.datastoreKindName)).countEntities(withLimit(10)));
        PreparedQuery profileQuery = ds.prepare(new Query(Profile.datastoreKindName));
        assertEquals(1, profileQuery.countEntities(withLimit(10)));
        Entity profileResultEntity = profileQuery.asList(withLimit(10)).get(0);
        assertEquals(profileID, profileResultEntity.getProperty(Profile.datastoreProfileIDName).toString());
        assertEquals(profileID, profileResultEntity.getKey().getName());

        PreparedQuery drinkQuery = ds.prepare(new Query(Drink.datastoreKindName));
        assertEquals(0, drinkQuery.countEntities(withLimit(10)));
    }

    @Test
    public void testAddDrink() {
        DatastoreService ds = DatastoreServiceFactory.getDatastoreService();
        ProfileInfoService service = new ProfileInfoService();
        String profileID = "TestProfileID";
        String drinkID = "TestDrinkID";

        service.addUser(new FullProfileRequest(profileID));

        Entity profileEntity = new Entity(Profile.datastoreKindName, profileID);
        profileEntity.setProperty(Profile.datastoreProfileIDName, profileID);
        Entity drinkEntity = new Entity(Drink.datastoreKindName, drinkID);
        drinkEntity.setProperty(Drink.datastoreDrinkIDName, drinkID);
        Profile newProfile = new Profile(profileEntity, new Drink[] { new Drink(drinkEntity , profileEntity.getKey())});
        service.syncUserInfo(new SyncProfileRequest(newProfile));

        assertEquals(1, ds.prepare(new Query(Profile.datastoreKindName)).countEntities(withLimit(10)));
        PreparedQuery profileQuery = ds.prepare(new Query(Profile.datastoreKindName));
        assertEquals(1, profileQuery.countEntities(withLimit(10)));
        Entity profileResultEntity = profileQuery.asList(withLimit(10)).get(0);
        assertEquals(profileID, profileResultEntity.getProperty(Profile.datastoreProfileIDName).toString());
        assertEquals(profileID, profileResultEntity.getKey().getName());

        PreparedQuery drinkQuery = ds.prepare(new Query(Drink.datastoreKindName));
        assertEquals(1, drinkQuery.countEntities(withLimit(10)));
        Entity drinkResultEntity = drinkQuery.asList(withLimit(10)).get(0);
        assertEquals(drinkID, drinkResultEntity.getProperty(Drink.datastoreDrinkIDName).toString());
        assertEquals(drinkID, drinkResultEntity.getKey().getName());
    }

    @Test
    public void testSimpleInsert() {
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
    public void testSimpleQuery() {
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
    public void testEmptyQuery() {
        String profileID = "TestProfileID";

        ProfileInfoService service = new ProfileInfoService();
        FullProfileResponse response = service.getUserInfo(new FullProfileRequest(profileID));
        assertEquals("Failure", response.getResult());
        assertNull(response.getProfile());
    }
}