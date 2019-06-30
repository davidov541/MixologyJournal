package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;

public class SyncProfileRequest {

    private Profile _profile;

    public SyncProfileRequest() {
    }

    SyncProfileRequest(Profile profile) {
        _profile = profile;
    }

    public Profile getProfile() { return _profile; }

    public void setProfile(Profile profile) { _profile = profile;}

    void setProfile(DatastoreService datastore) {
        _profile.save(datastore);
    }
}
