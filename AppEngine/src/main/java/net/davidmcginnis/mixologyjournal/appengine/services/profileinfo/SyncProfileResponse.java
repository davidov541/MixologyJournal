package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

public class SyncProfileResponse {

    private String _result;

    SyncProfileResponse(String result) {
        _result = result;
    }

    public String getResult() {
        return _result;
    }

    public void setResult(String result) {
        _result = result;
    }
}
