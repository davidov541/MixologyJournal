package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.appengine.api.datastore.*;

import java.util.List;

public class Profile {
    private String _profileID;
    private Drink[] _drinks;

    static final String datastoreKindName = "User";
    static final String datastoreProfileIDName = "email";

    private Profile(Entity entity, Drink[] drinks) {
        this._profileID = (String)entity.getProperty(Profile.datastoreProfileIDName);
        this._drinks = drinks;
    }

    static Profile createProfile(DatastoreService datastore, Key profileKey) throws EntityNotFoundException {
        Entity result = datastore.get(profileKey);

        Query drinkQuery = new Query(Drink.datastoreKindName).setAncestor(result.getKey());
        List<Entity> drinkEntities = datastore.prepare(drinkQuery).asList(FetchOptions.Builder.withDefaults());
        Drink[] drinks = new Drink[drinkEntities.size()];
        for (int i = 0; i < drinks.length; i++) {
            drinks[i] = Drink.createDrink(datastore, drinkEntities.get(i));
        }

        return new Profile(result, drinks);
    }

    void save(DatastoreService datastore) {
        Entity newEntity = new Entity(datastoreKindName, _profileID);

        newEntity.setProperty(datastoreProfileIDName, _profileID);

        datastore.put(newEntity);
    }

    public String getProfileID() {
        return _profileID;
    }

    public void setProfileID(String profileID) {
        _profileID = profileID;
    }

    public Drink[] getDrinks() {
        return _drinks;
    }

    public void setDrinks(Drink[] drinks) {
        _drinks = drinks;
    }
}
