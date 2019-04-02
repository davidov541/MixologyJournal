using System;
using GeoCoordinatePortable;

namespace MixologyJournalApp.ViewModel.LocationServices
{
    internal class NearbyPlace : INearbyPlace
    {
        public String Name
        {
            get;
            private set;
        }

        internal String Identifier
        {
            get;
            private set;
        }

        internal double Lattitude
        {
            get
            {
                return _location.Latitude;
            }
        }

        internal double Longitude
        {
            get
            {
                return _location.Longitude;
            }
        }

        private GeoCoordinate _location;
        internal NearbyPlace(String name, String identifier, GeoCoordinate location)
        {
            Name = name;
            Identifier = identifier;
            _location = location;
        }

        internal double GetDistanceFromPoint(GeoCoordinate point)
        {
            return point.GetDistanceTo(_location);
        }

        public override Boolean Equals(Object obj)
        {
            NearbyPlace other = obj as NearbyPlace;
            if (other == null)
            {
                return false;
            }
            if (!other.Identifier.Equals(Identifier))
            {
                return false;
            }
            return true;
        }

        public override Int32 GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}
