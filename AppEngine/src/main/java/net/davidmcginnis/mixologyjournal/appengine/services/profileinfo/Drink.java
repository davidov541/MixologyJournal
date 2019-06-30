package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;
import com.google.appengine.repackaged.com.google.datastore.v1.Datastore;

public class Drink {
    private String _drinkID;
    private Key _profileKey;
    private Recipe _derivedRecipe;
    private Key _derivedRecipeKey;

    static final String datastoreKindName = "Drink";
    static final String datastoreDrinkIDName = "drinkID";
    static final String datastoreDerivedRecipeName = "recipeKey";

    private Drink(Entity entity, Key profileKey, Recipe derivedRecipe) {
        this._drinkID = (String)entity.getProperty(Drink.datastoreDrinkIDName);
        this._profileKey = profileKey;
        this._derivedRecipe = derivedRecipe;
        this._derivedRecipeKey = _derivedRecipe.getRecipeKey();
    }

    private Drink(String drinkID, Key profileKey, Recipe derivedRecipe) {
        this._drinkID = drinkID;
        this._profileKey = profileKey;
        this._derivedRecipe = derivedRecipe;
        this._derivedRecipeKey = _derivedRecipe.getRecipeKey();
    }

    static Drink getDrink(DatastoreService datastore, Entity drink, Key profileKey) throws EntityNotFoundException {
        Key recipeKey = (Key)drink.getProperty(datastoreDerivedRecipeName);
        Recipe derivedRecipe = Recipe.getRecipe(datastore, recipeKey);

        return new Drink(drink, profileKey, derivedRecipe);
    }

    static Drink createDrink(String drinkID, Key profileKey, Recipe derivedRecipe) {
        return new Drink(drinkID, profileKey, derivedRecipe);
    }

    void save(DatastoreService datastore) {
        Entity entity = new Entity(datastoreKindName, _drinkID, _profileKey);

        _derivedRecipe.save(datastore);

        entity.setProperty(datastoreDrinkIDName, _drinkID);
        entity.setProperty(datastoreDerivedRecipeName, _derivedRecipe.getRecipeKey());

        datastore.put(entity);
    }

    public String getDrinkID() {
        return _drinkID;
    }

    public void setDrinkID(String drinkID) {
        _drinkID = drinkID;
    }

    public Key getDerivedRecipeKey() {
        return _derivedRecipeKey;
    }

    public void setDerivedRecipeKey(Key derivedRecipeKey) {
        _derivedRecipeKey = derivedRecipeKey;
    }
}
