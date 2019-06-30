package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.EntityNotFoundException;
import com.google.appengine.api.datastore.Key;
import com.google.appengine.repackaged.com.google.datastore.v1.Datastore;

public class Drink {
    private String _drinkID;
    private Key _profileKey;

    static final String datastoreKindName = "Drink";
    static final String datastoreDrinkIDName = "drinkID";

    private Drink(Entity entity, Key profileKey) {
        this._drinkID = (String)entity.getProperty(Drink.datastoreDrinkIDName);
        this._profileKey = profileKey;
    }

    private Drink(String drinkID, Key profileKey) {
        this._drinkID = drinkID;
        this._profileKey = profileKey;
    }

    static Drink createDrink(DatastoreService datastore, Entity drink, Key profileKey) {
        return new Drink(drink, profileKey);
    }

    static Drink createDrink(String drinkID, Key profileKey) {
        return new Drink(drinkID, profileKey);
    }

    void save(DatastoreService datastore) {
        Entity entity = new Entity(datastoreKindName, _drinkID, _profileKey);

        entity.setProperty(datastoreDrinkIDName, _drinkID);

        datastore.put(entity);
    }

    public String getRecipeID() {
        return _drinkID;
    }

    public void setRecipeID(String profileID) {
        _drinkID = profileID;
    }
}
