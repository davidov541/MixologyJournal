using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.Persistence
{
    [TestClass]
    public class ModifiedRecipeEntryPersisterUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"" Rating=""2.5"" RecipeID=""5"">
                  <Picture Path = ""Picture1"" />
                  <Picture Path=""Picture2"" />
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(BasicEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Text Journal", createdEntry.Title);
            Assert.AreEqual("Contents", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);
            Assert.AreEqual(2.5, createdEntry.Rating);

            // Check Pictures
            Assert.AreEqual(2, createdEntry.Pictures.Count());
            Assert.IsTrue(createdEntry.Pictures.Contains("Picture1"));
            Assert.IsTrue(createdEntry.Pictures.Contains("Picture2"));
        }

        private const String NoPicturesEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"" Rating=""2.5"" RecipeID=""5"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void NoPicturesLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoPicturesEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Text Journal", createdEntry.Title);
            Assert.AreEqual("Contents", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);
            Assert.AreEqual(2.5, createdEntry.Rating);

            // Check Pictures
            Assert.AreEqual(0, createdEntry.Pictures.Count());
        }

        private const String InvalidRecipeIDEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"" Rating=""2.5"" RecipeID=""10000"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void InvalidRecipeIDLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(InvalidRecipeIDEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoRecipeIDEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"" Rating=""2.5"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void NoRecipeIDLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoRecipeIDEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoRatingEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"" RecipeID=""5"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void NoRatingLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoRatingEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Text Journal", createdEntry.Title);
            Assert.AreEqual("Contents", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);
            Assert.AreEqual(0.0, createdEntry.Rating);

            // Check Pictures
            Assert.AreEqual(0, createdEntry.Pictures.Count());
        }

        private const String NoCreationDateEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" Contents=""Contents"" Rating=""2.5"" RecipeID=""5"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void NoCreationDateLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoCreationDateEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoContentsEntry = @"<ModifiedRecipeJournalEntry Title=""Text Journal"" CreationDate=""100"" Rating=""2.5"" RecipeID=""5"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void NoContentsLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoContentsEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoTitleEntry = @"<ModifiedRecipeJournalEntry Contents=""Contents"" CreationDate=""100"" Rating=""2.5"" RecipeID=""5"">
                </ModifiedRecipeJournalEntry>";
        [TestMethod]
        public void NoTitleLoadTest()
        {
            // Create the journal along with the recipe we need.
            Journal journal = new Journal();
            BaseRecipe baseRecipe = new BaseRecipe("Title", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, 5);
            baseRecipe.AddModifiedRecipe(recipe);
            journal.AddRecipe(baseRecipe);

            // Run the function.
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            HomemadeDrinkEntry createdEntry;
            XDocument doc = XDocument.Parse(NoTitleEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        #endregion

        #region Saving Tests
        [TestMethod]
        public void BasicSaveTest()
        {
            DateTime date = new DateTime(100);
            BaseRecipe baseRecipe = new BaseRecipe("Text Journal", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            HomemadeDrinkEntry entry = new HomemadeDrinkEntry(recipe, "Contents", date);
            entry.Rating = 2.5;
            entry.AddPicture("Picture1");
            entry.AddPicture("Picture2");

            XDocument doc = new XDocument();
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            persister.Write(entry, doc);

            XDocument expectedDoc = XDocument.Parse(BasicEntry);

            CompareXml(expectedDoc, doc);
        }

        [TestMethod]
        public void NoPicturesSaveTest()
        {
            DateTime date = new DateTime(100);
            BaseRecipe baseRecipe = new BaseRecipe("Text Journal", "Instructions");
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            HomemadeDrinkEntry entry = new HomemadeDrinkEntry(recipe, "Contents", date);
            entry.Rating = 2.5;

            XDocument doc = new XDocument();
            ModifiedRecipeEntryPersister persister = new ModifiedRecipeEntryPersister();
            persister.Write(entry, doc);

            XDocument expectedDoc = XDocument.Parse(NoPicturesEntry);

            CompareXml(expectedDoc, doc);
        }
        #endregion
    }
}
