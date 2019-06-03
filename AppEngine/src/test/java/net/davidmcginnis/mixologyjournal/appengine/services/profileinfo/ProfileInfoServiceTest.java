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