package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

public class AddUserResponse {

    private String _result;
    private String _profileID;

    AddUserResponse(String result, String profileID) {
        _result = result;
        _profileID = profileID;
    }

    public String getResult() {
        return _result;
    }

    public void setResult(String result) {
        _result = result;
    }

    public String getProfileID() { return _profileID; }

    public void setProfile(String profileID) { _profileID = profileID;}
}
