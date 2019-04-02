using System;
using System.Linq;
using System.Xml.Linq;
using GeoCoordinatePortable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournalApp.Test.Persistence
{
    [TestClass]
    public class BarDrinkEntryUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicEntry = @"<BarDrinkEntry Title=""Name"" Contents=""Notes"" CreationDate=""100"">
                <Picture Path = ""Picture1"" />
                <Picture Path=""Picture2"" />
                <NearbyPlace Name=""Name"" Identifier=""Identifier"" Lattitude=""5.5"" Longitude=""-10.2"" />
                <BarDrink Title=""Name"" Instructions=""Instructions"" ID=""0"">
                </BarDrink>
                </BarDrinkEntry>";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            BarDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(BasicEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Name", createdEntry.Title);
            Assert.AreEqual("Notes", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);

            // Check Pictures
            Assert.AreEqual(2, createdEntry.Pictures.Count());
            Assert.IsTrue(createdEntry.Pictures.Contains("Picture1"));
            Assert.IsTrue(createdEntry.Pictures.Contains("Picture2"));

            // Check Location
            Assert.AreEqual("Name", createdEntry.Location.Name);
            Assert.AreEqual("Identifier", (createdEntry.Location as NearbyPlace).Identifier);
            Assert.AreEqual(5.5, (createdEntry.Location as NearbyPlace).Lattitude);
            Assert.AreEqual(-10.2, (createdEntry.Location as NearbyPlace).Longitude);
        }

        private const String NoPicturesEntry = @"<BarDrinkEntry Title=""Name"" Contents=""Notes"" CreationDate=""100"">
                <BarDrink Title=""Name"" Instructions=""Instructions"" ID=""0"">
                </BarDrink>
                </BarDrinkEntry>";
        [TestMethod]
        public void NoPicturesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            BarDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoPicturesEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Name", createdEntry.Title);
            Assert.AreEqual("Notes", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);

            // Check Pictures
            Assert.AreEqual(0, createdEntry.Pictures.Count());
        }

        private const String NoCreationDateEntry = @"<BarDrinkEntry Title=""Text Journal"" Contents=""Contents"">
                <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
                <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
                <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
                </BarDrink>
                </BarDrinkEntry>";
        [TestMethod]
        public void NoCreationDateLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            BarDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoCreationDateEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoContentsEntry = @"<BarDrinkEntry Title=""Text Journal"" CreationDate=""100"">
                <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
                <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
                <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
                </BarDrink>
                </BarDrinkEntry>";
        [TestMethod]
        public void NoContentsLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            BarDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoContentsEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoTitleEntry = @"<BarDrinkEntry Contents=""Contents"" CreationDate=""100"">
                <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
                <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
                <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
                </BarDrink>
                </BarDrinkEntry>";
        [TestMethod]
        public void NoTitleLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            BarDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoTitleEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoRecipeEntry = @"<BarDrinkEntry Contents=""Contents"" CreationDate=""100"">
                </BarDrinkEntry>";
        [TestMethod]
        public void NoRecipeLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            BarDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoRecipeEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }
        #endregion

        #region Saving Tests
        [TestMethod]
        public void BasicSaveTest()
        {
            DateTime date = new DateTime(100);
            BarDrink drink = new BarDrink();
            drink.Name = "Name";
            drink.Instructions = "Instructions";
            BarDrinkEntry entry = new BarDrinkEntry(drink, "Notes", date);
            entry.AddPicture("Picture1");
            entry.AddPicture("Picture2");
            entry.Location = new NearbyPlace("Name", "Identifier", new GeoCoordinate(5.5, -10.2));

            XDocument doc = new XDocument();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            persister.Write(entry, doc);

            XDocument expectedDoc = XDocument.Parse(BasicEntry);

            CompareXml(expectedDoc, doc);
        }

        [TestMethod]
        public void NoPicturesSaveTest()
        {
            DateTime date = new DateTime(100);
            BarDrink drink = new BarDrink();
            drink.Name = "Name";
            drink.Instructions = "Instructions";
            BarDrinkEntry entry = new BarDrinkEntry(drink, "Notes", date);

            XDocument doc = new XDocument();
            BarDrinkEntryPersister persister = new BarDrinkEntryPersister();
            persister.Write(entry, doc);

            XDocument expectedDoc = XDocument.Parse(NoPicturesEntry);

            CompareXml(expectedDoc, doc);
        }
        #endregion
    }
}
