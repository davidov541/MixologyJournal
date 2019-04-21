using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using GeoCoordinatePortable;

namespace MixologyJournalApp.ViewModel.LocationServices
{
    internal class GooglePlacesProvider : INearbyPlacesProvider
    {
        public async Task<IEnumerable<INearbyPlace>> GetNearbyPlacesAsync()
        {
            // TODO: Handle errors from exceptions.
            /*
            IGeolocator locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            GeoPosition position = await locator.GetPositionAsync(10000);
            GeoCoordinate coordinate = new GeoCoordinate(position.Latitude, position.Longitude);
            */
            List<NearbyPlace> places = new List<NearbyPlace>();
            /*
            places.AddRange(await GetNearbyPlacesOfTypeAsync("bar", coordinate));
            places.AddRange(await GetNearbyPlacesOfTypeAsync("restaurant", coordinate));
            places.Sort((p1, p2) => p1.GetDistanceFromPoint(coordinate).CompareTo(p2.GetDistanceFromPoint(coordinate)));
            */
            return places.Distinct();
        }

        private async Task<IEnumerable<NearbyPlace>> GetNearbyPlacesOfTypeAsync(String locationType, GeoCoordinate position)
        {
            String requestUrl = String.Format(CultureInfo.InvariantCulture,
                            "https://maps.googleapis.com/maps/api/place/nearbysearch/xml?location={0},{1}&rankby=distance&type={2}&key=AIzaSyAnEVyKnESsdslpjJ-reI-MqG59vJEsugc",
                            position.Latitude,
                            position.Longitude,
                            locationType);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            List<NearbyPlace> places = new List<NearbyPlace>();
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                // TODO: Check response for errors.
                XDocument doc = XDocument.Load(response.GetResponseStream());
                XElement results = doc.Element(XName.Get("PlaceSearchResponse"));
                foreach (XElement result in results.Elements("result"))
                {
                    String name = result.Element("name").Value;
                    String identifier = result.Element("place_id").Value;
                    double longitude = Double.Parse(result.Element("geometry").Element("location").Element("lng").Value);
                    double lattitude = Double.Parse(result.Element("geometry").Element("location").Element("lat").Value);
                    places.Add(new NearbyPlace(name, identifier, position));
                }
            }
            return places;
        }
    }
}
