package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

public class FullProfileResponse {

    private String _result;
    private Profile _profile;

    FullProfileResponse(String result, Profile profile) {
        _result = result;
        _profile = profile;
    }

    public String getResult() {
        return _result;
    }

    public void setResult(String result) {
        _result = result;
    }

    public Profile getProfile() { return _profile; }

    public void setProfile(Profile profile) { _profile = profile;}
}
