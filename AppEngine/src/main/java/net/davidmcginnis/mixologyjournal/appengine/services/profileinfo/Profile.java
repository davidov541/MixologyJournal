package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.*;

import java.util.List;

public class Profile {
    private String _profileID;
    private Recipe[] _recipes;

    static final String datastoreKindName = "User";
    static final String datastoreProfileIDName = "email";

    private Profile(Entity entity, Recipe[] drinks) {
        this._profileID = (String)entity.getProperty(Profile.datastoreProfileIDName);
        this._recipes = drinks;
    }

    private Profile(String profileID, Recipe[] drinks) {
        this._profileID = profileID;
        this._recipes = drinks;
    }

    static Profile createProfile(DatastoreService datastore, Key profileKey) throws EntityNotFoundException {
        Entity result = datastore.get(profileKey);

        Query recipeQuery = new Query(Recipe.datastoreKindName).setAncestor(result.getKey());
        List<Entity> recipeEntities = datastore.prepare(recipeQuery).asList(FetchOptions.Builder.withDefaults());
        Recipe[] recipes = new Recipe[recipeEntities.size()];
        for (int i = 0; i < recipes.length; i++) {
            recipes[i] = Recipe.getRecipe(datastore, recipeEntities.get(i), profileKey);
        }

        return new Profile(result, recipes);
    }

    static Profile createProfile(String profileID, Recipe[] recipes) {
        return new Profile(profileID, recipes);
    }

    void save(DatastoreService datastore) {
        Entity newEntity = new Entity(datastoreKindName, _profileID);

        newEntity.setProperty(datastoreProfileIDName, _profileID);

        for(Recipe recipe: _recipes) {
            recipe.save(datastore);
        }

        datastore.put(newEntity);
    }

    public String getProfileID() {
        return _profileID;
    }

    public void setProfileID(String profileID) {
        _profileID = profileID;
    }

    public Recipe[] getRecipes() {
        return _recipes;
    }

    public void setRecipes(Recipe[] recipes) {
        _recipes = recipes;
    }
}
