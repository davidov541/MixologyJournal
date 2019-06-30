package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.DatastoreService;
import com.google.appengine.api.datastore.Entity;
import com.google.appengine.api.datastore.Key;

public class Drink {
    private String _drinkID;
    private Key _recipeKey;

    static final String kindName = "Drink";
    static final String datastoreDrinkIDName = "drinkID";

    private Drink(Entity entity, Key recipeKey) {
        this._drinkID = (String)entity.getProperty(Drink.datastoreDrinkIDName);
    }

    private Drink(String drinkID, Key recipeKey) {
        this._drinkID = drinkID;
        this._recipeKey = recipeKey;
    }

    static Drink get(DatastoreService datastore, Entity drink, Key recipeKey) {
        return new Drink(drink, recipeKey);
    }

    static Drink create(String drinkID, Key recipeKey) {
        return new Drink(drinkID, recipeKey);
    }

    void save(DatastoreService datastore) {
        Entity entity = new Entity(kindName, _drinkID, _recipeKey);

        entity.setProperty(datastoreDrinkIDName, _drinkID);

        datastore.put(entity);
    }

    boolean equals(Entity entity) {
        boolean areEqual = _drinkID.equals(entity.getProperty(Drink.datastoreDrinkIDName).toString());
        areEqual = areEqual && _drinkID.equals(entity.getKey().getName());
        return areEqual;
    }

    public String getDrinkID() {
        return _drinkID;
    }

    public void setDrinkID(String drinkID) {
        _drinkID = drinkID;
    }
}
