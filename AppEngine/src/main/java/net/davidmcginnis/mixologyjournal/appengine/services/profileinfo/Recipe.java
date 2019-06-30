package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;

public class Recipe {
    private String _recipeID;
    static final String datastoreKindName = "Recipe";
    private static final String datastoreRecipeIDName = "recipeID";

    private Recipe(Entity entity) {
        this._recipeID = (String)entity.getProperty(Recipe.datastoreRecipeIDName);
    }

    private Recipe(String recipeID) {
        this._recipeID = recipeID;
    }

    static Recipe getRecipe(DatastoreService datastore, Key recipeKey) throws EntityNotFoundException {
        Entity result = datastore.get(recipeKey);
        return new Recipe(result);
    }

    static Recipe createRecipe(String recipeID) {
        return new Recipe(recipeID);
    }

    public String getRecipeID() {
        return _recipeID;
    }

    public void setRecipeID(String profileID) {
        _recipeID = profileID;
    }

    public Key getRecipeKey() {
        Entity recipeEntity = new Entity(datastoreKindName, _recipeID);
        return recipeEntity.getKey();
    }

    public void save(DatastoreService datastore) {
        Entity entity = new Entity(datastoreKindName, _recipeID);

        entity.setProperty(datastoreRecipeIDName, _recipeID);

        datastore.put(entity);
    }
}
