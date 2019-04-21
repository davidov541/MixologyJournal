using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournalApp.Test.Persistence
{
    [TestClass]
    public class NearbyPlacePersisterUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicEntry = @"<NearbyPlace Name=""Name"" Identifier=""Identifier"" Lattitude=""5.5"" Longitude=""-10.2"" />";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            NearbyPlacePersister persister = new NearbyPlacePersister();
            INearbyPlace createdEntry;
            XDocument doc = XDocument.Parse(BasicEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Name", createdEntry.Name);
            Assert.AreEqual("Identifier", (createdEntry as NearbyPlace).Identifier);
            Assert.AreEqual(5.5, (createdEntry as NearbyPlace).Lattitude);
            Assert.AreEqual(-10.2, (createdEntry as NearbyPlace).Longitude);
        }

        private const String NoNameEntry = @"<NearbyPlace Identifier=""Identifier"" Lattitude=""5.5"" Longitude=""-10.2"" />";
        [TestMethod]
        public void NoNameLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            NearbyPlacePersister persister = new NearbyPlacePersister();
            INearbyPlace createdEntry;
            XDocument doc = XDocument.Parse(NoNameEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoIdentifierEntry = @"<NearbyPlace Name=""Name"" Lattitude=""5.5"" Longitude=""-10.2"" />";
        [TestMethod]
        public void NoIdentifierLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            NearbyPlacePersister persister = new NearbyPlacePersister();
            INearbyPlace createdEntry;
            XDocument doc = XDocument.Parse(NoIdentifierEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoLattitudeEntry = @"<NearbyPlace Name=""Name"" Identifier=""Identifier"" Longitude=""-10.2"" />";
        [TestMethod]
        public void NoLattitudeLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            NearbyPlacePersister persister = new NearbyPlacePersister();
            INearbyPlace createdEntry;
            XDocument doc = XDocument.Parse(NoLattitudeEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoLongitudeEntry = @"<NearbyPlace Name=""Name"" Identifier=""Identifier"" Lattitude=""5.5"" />";
        [TestMethod]
        public void NoLongitudeLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            NearbyPlacePersister persister = new NearbyPlacePersister();
            INearbyPlace createdEntry;
            XDocument doc = XDocument.Parse(NoLongitudeEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }
        #endregion

        #region Saving Tests
        /*
        [TestMethod]
        public void BasicSaveTest()
        {
            DateTime date = new DateTime(100);
            String name = "Name";
            String identifier = "Identifier";
            GeoCoordinate location = new GeoCoordinate(5.5, -10.2);
            NearbyPlace place = new NearbyPlace(name, identifier, location);

            XDocument doc = new XDocument();
            NearbyPlacePersister persister = new NearbyPlacePersister();
            persister.Write(place, doc);

            XDocument expectedDoc = XDocument.Parse(BasicEntry);

            CompareXml(expectedDoc, doc);
        }
        */
        #endregion
    }
}
