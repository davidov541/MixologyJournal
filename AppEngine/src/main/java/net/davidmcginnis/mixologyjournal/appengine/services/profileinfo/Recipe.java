package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.*;

import java.util.List;

public class Recipe {
    private String _recipeID;
    private Key _profileKey;
    private Drink[] _drinks;

    static final String datastoreKindName = "Recipe";
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

    static Recipe getRecipe(DatastoreService datastore, Entity recipe, Key profileKey) throws EntityNotFoundException {
        Query drinkQuery = new Query(Recipe.datastoreKindName).setAncestor(recipe.getKey());
        List<Entity> drinkEntities = datastore.prepare(drinkQuery).asList(FetchOptions.Builder.withDefaults());
        Drink[] drinks = new Drink[drinkEntities.size()];
        for (int i = 0; i < drinks.length; i++) {
            drinks[i] = Drink.getDrink(datastore, drinkEntities.get(i), recipe.getKey());
        }

        return new Recipe(recipe, profileKey, drinks);
    }

    static Recipe createRecipe(String recipeID, Key profileKey, Drink[] drinks) {
        return new Recipe(recipeID, profileKey, drinks);
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
        Entity entity = new Entity(datastoreKindName, _recipeID, _profileKey);

        entity.setProperty(datastoreRecipeIDName, _recipeID);

        for(Drink drink: _drinks) {
            drink.save(datastore);
        }

        datastore.put(entity);
    }
}
