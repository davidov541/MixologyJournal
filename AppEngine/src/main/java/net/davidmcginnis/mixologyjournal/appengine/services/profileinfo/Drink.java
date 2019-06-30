package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;

public class Drink {
    private String _drinkID;
    private Key _recipeKey;

    static final String datastoreKindName = "Drink";
    static final String datastoreDrinkIDName = "drinkID";
    static final String datastoreDerivedRecipeName = "recipeKey";

    private Drink(Entity entity, Key recipeKey) {
        this._drinkID = (String)entity.getProperty(Drink.datastoreDrinkIDName);
    }

    private Drink(String drinkID, Key recipeKey) {
        this._drinkID = drinkID;
        this._recipeKey = recipeKey;
    }

    static Drink getDrink(DatastoreService datastore, Entity drink, Key recipeKey) {
        return new Drink(drink, recipeKey);
    }

    static Drink createDrink(String drinkID, Key recipeKey) {
        return new Drink(drinkID, recipeKey);
    }

    void save(DatastoreService datastore) {
        Entity entity = new Entity(datastoreKindName, _drinkID, _recipeKey);

        entity.setProperty(datastoreDrinkIDName, _drinkID);

        datastore.put(entity);
    }

    public String getDrinkID() {
        return _drinkID;
    }

    public void setDrinkID(String drinkID) {
        _drinkID = drinkID;
    }
}
