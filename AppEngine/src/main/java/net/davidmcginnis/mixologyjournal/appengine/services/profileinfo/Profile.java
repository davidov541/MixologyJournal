package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;

public class Profile {
    private String _profileID;
    static final String datastoreKindName = "User";
    static final String datastoreProfileIDName = "email";

    private Profile(Entity entity) {
        this._profileID = (String)entity.getProperty(Profile.datastoreProfileIDName);
    }

    static Profile createProfile(DatastoreService datastore, Key profileKey) throws EntityNotFoundException {
        Entity result = datastore.get(profileKey);
        return new Profile(result);
    }

    public String getProfileID() {
        return _profileID;
    }

    public void setProfileID(String profileID) {
        _profileID = profileID;
    }
}
