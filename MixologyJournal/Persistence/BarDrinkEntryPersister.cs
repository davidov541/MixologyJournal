using System;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournal.Persistence
{
    internal class BarDrinkEntryPersister : JournalEntryPersister, ISettingsPersister<BarDrinkEntry>
    {
        private const String PersistedName = "BarDrinkEntry";
        private BarDrinkPersister _drinkPersister;
        private NearbyPlacePersister _locationPersister;

        public BarDrinkEntryPersister()
        {
            _drinkPersister = new BarDrinkPersister();
            _locationPersister = new NearbyPlacePersister();
        }

        public Boolean TryCreate(XElement element, Journal journal, out BarDrinkEntry createdElement)
        {
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName))
            {
                BarDrink drink;
                bool success = false;
                INearbyPlace location = null;
                foreach (XElement elem in element.Elements())
                {
                    if (_drinkPersister.TryCreate(elem, journal, out drink))
                    {
                        createdElement = new BarDrinkEntry(drink);
                        success = TryPopulateInstance(createdElement, element);
                    }
                    else if (_locationPersister.TryCreate(elem, journal, out location))
                    {
                        // Do nothing here since we can't guarantee BarDrinkEntry has been created yet.
                    }
                }
                if (createdElement != null)
                {
                    createdElement.Location = location;
                }
                return success;
            }
            return false;
        }

        public void Write(BarDrinkEntry elementToPersist, XContainer parentNode)
        {
            XElement entryElem = new XElement(PersistedName);
            PopulateXmlElement(entryElem, elementToPersist, parentNode);
            _drinkPersister.Write(elementToPersist.Recipe, entryElem);
            if (elementToPersist.Location != null)
            {
                _locationPersister.Write(elementToPersist.Location, entryElem);
            }
            parentNode.Add(entryElem);
        }
    }
}
