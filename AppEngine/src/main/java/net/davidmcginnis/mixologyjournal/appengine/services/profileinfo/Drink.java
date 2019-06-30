package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;

public class Drink {
    private String _drinkID;
    static final String datastoreKindName = "Drink";
    private static final String datastoreDrinkIDName = "drinkID";

    private Drink(Entity entity) {
        this._drinkID = (String)entity.getProperty(Drink.datastoreDrinkIDName);
    }

    static Drink createDrink(DatastoreService datastore, Entity drink) {
        return new Drink(drink);
    }

    public String getRecipeID() {
        return _drinkID;
    }

    public void setRecipeID(String profileID) {
        _drinkID = profileID;
    }
}
