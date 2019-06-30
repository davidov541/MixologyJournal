package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;

public class Recipe {
    private String _recipeID;
    static final String datastoreKindName = "Recipe";
    static final String datastoreRecipeIDName = "recipeID";

    private Recipe(Entity entity) {
        this._recipeID = (String)entity.getProperty(Recipe.datastoreRecipeIDName);
    }

    static Recipe createProfile(DatastoreService datastore, Key profileKey) throws EntityNotFoundException {
        Entity result = datastore.get(profileKey);
        return new Recipe(result);
    }

    public String getRecipeID() {
        return _recipeID;
    }

    public void setRecipeID(String profileID) {
        _recipeID = profileID;
    }
}
