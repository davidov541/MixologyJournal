using System;
using System.Xml.Linq;
using GeoCoordinatePortable;
using MixologyJournal.SourceModel;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournal.Persistence
{
    internal class NearbyPlacePersister : ISettingsPersister<INearbyPlace>
    {
        private const String PersistedName = "NearbyPlace";
        private const String PersistedLocationName = "Name";
        private const String PersistedIdentifier = "Identifier";
        private const String PersistedLatitude = "Lattitude";
        private const String PersistedLongitude = "Longitude";

        public Boolean TryCreate(XElement element, Journal journal, out INearbyPlace createdElement)
        {
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName))
            {
                double lat, lng;
                if (element.Attribute(PersistedLocationName) != null &&
                    element.Attribute(PersistedIdentifier) != null &&
                    element.Attribute(PersistedLatitude) != null &&
                    element.Attribute(PersistedLongitude) != null &&
                    Double.TryParse(element.Attribute(PersistedLatitude).Value, out lat) &&
                    Double.TryParse(element.Attribute(PersistedLongitude).Value, out lng))
                {
                    String name = element.Attribute(PersistedLocationName).Value;
                    String identifier = element.Attribute(PersistedIdentifier).Value;
                    createdElement = new NearbyPlace(name, identifier, new GeoCoordinate(lat, lng));
                    return true;
                }
            }
            return false;
        }

        public void Write(INearbyPlace elementToPersist, XContainer parentNode)
        {
            NearbyPlace place = elementToPersist as NearbyPlace;
            XElement entryElem = new XElement(PersistedName);
            entryElem.SetAttributeValue(PersistedLocationName, elementToPersist.Name);
            entryElem.SetAttributeValue(PersistedIdentifier, place.Identifier);
            entryElem.SetAttributeValue(PersistedLatitude, place.Lattitude);
            entryElem.SetAttributeValue(PersistedLongitude, place.Longitude);
            parentNode.Add(entryElem);
        }
    }
}
