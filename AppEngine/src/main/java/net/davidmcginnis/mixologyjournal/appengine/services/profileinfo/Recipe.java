package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.*;

import java.util.List;

public class Recipe {
    private String _recipeID;
    private Key _profileKey;
    private Drink[] _drinks;

    static final String kindName = "Recipe";
    static final String datastoreRecipeIDName = "recipeID";

    private Recipe(Entity entity, Key profileKey, Drink[] drinks) {
        this._recipeID = (String)entity.getProperty(Recipe.datastoreRecipeIDName);
        this._profileKey = profileKey;
        this._drinks = drinks;
    }

    private Recipe(String recipeID, Key profileKey, Drink[] drinks) {
        this._recipeID = recipeID;
        this._profileKey = profileKey;
        this._drinks = drinks;
    }

    static Recipe get(DatastoreService datastore, Entity recipe, Key profileKey) {
        Query drinkQuery = new Query(Recipe.kindName).setAncestor(recipe.getKey());
        List<Entity> drinkEntities = datastore.prepare(drinkQuery).asList(FetchOptions.Builder.withDefaults());
        Drink[] drinks = new Drink[drinkEntities.size()];
        for (int i = 0; i < drinks.length; i++) {
            drinks[i] = Drink.get(datastore, drinkEntities.get(i), recipe.getKey());
        }

        return new Recipe(recipe, profileKey, drinks);
    }

    static Recipe create(String recipeID, Key profileKey, Drink[] drinks) {
        return new Recipe(recipeID, profileKey, drinks);
    }

    boolean equals(Entity entity) {
        boolean areEqual = _recipeID.equals(entity.getProperty(Recipe.datastoreRecipeIDName).toString());
        areEqual = areEqual && _recipeID.equals(entity.getKey().getName());
        areEqual = areEqual && _profileKey.equals(entity.getKey().getParent());
        return areEqual;
    }

    public String getRecipeID() {
        return _recipeID;
    }

    public Drink[] getDrinks() {
        return _drinks;
    }

    public void setRecipeID(String profileID) {
        _recipeID = profileID;
    }

    void save(DatastoreService datastore) {
        Entity entity = new Entity(kindName, _recipeID, _profileKey);

        entity.setProperty(datastoreRecipeIDName, _recipeID);

        for(Drink drink: _drinks) {
            drink.save(datastore);
        }

        datastore.put(entity);
    }
}
