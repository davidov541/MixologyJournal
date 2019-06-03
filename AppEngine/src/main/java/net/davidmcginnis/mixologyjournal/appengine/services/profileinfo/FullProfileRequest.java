package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;

public class FullProfileRequest {

    private String _profileID;

    public FullProfileRequest() {
    }

    public FullProfileRequest(String profileID) {
        _profileID = profileID;
    }

    public String getProfileID() {
        return _profileID;
    }

    public void setProfileID(String profileID) {
        _profileID = profileID;
    }

    Profile getProfile(DatastoreService datastore) throws EntityNotFoundException {
        Entity possibleEntity = new Entity(Profile.datastoreKindName, getProfileID());
        return Profile.createProfile(datastore, possibleEntity.getKey());
    }
}
