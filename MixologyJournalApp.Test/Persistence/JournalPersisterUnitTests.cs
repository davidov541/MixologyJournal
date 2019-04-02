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
    public class JournalPersisterUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicEntry = @"<Journal>
                    <BaseRecipe Title="""" Instructions="""" ID=""0"">
                    <DerivedRecipe Title="""" Instructions="""" ID=""1"">
                    </DerivedRecipe>
                    </BaseRecipe>
                    <TextJournalEntry Title="""" Contents="""" CreationDate=""100"">
                    </TextJournalEntry>
                    <ModifiedRecipeJournalEntry Title="""" Contents="""" CreationDate=""100"" Rating=""0"" RecipeID=""1"">
                    </ModifiedRecipeJournalEntry>
                    </Journal>";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Run the function.
            JournalPersister persister = new JournalPersister();
            Journal createdJournal;
            XDocument doc = XDocument.Parse(BasicEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), null, out createdJournal));

            // Check entries
            Assert.AreEqual(1, createdJournal.BaseRecipes.Count());
            Assert.AreEqual(2, createdJournal.Entries.Count());
            Assert.AreEqual(1, createdJournal.Entries.OfType<NoteEntry>().Count());
            Assert.AreEqual(1, createdJournal.Entries.OfType<HomemadeDrinkEntry>().Count());
            Assert.IsTrue(ReferenceEquals(createdJournal.BaseRecipes.First().DerivedRecipes.First(), createdJournal.Entries.OfType<HomemadeDrinkEntry>().First().Recipe));
        }

        private const String MisorderedEntry = @"<Journal>
                    <ModifiedRecipeJournalEntry Title="""" Contents="""" CreationDate=""100"" Rating=""0"" RecipeID=""1"">
                    </ModifiedRecipeJournalEntry>
                    <BaseRecipe Title="""" Instructions="""">
                    <DerivedRecipe Title="""" Instructions="""" ID=""1"">
                    </DerivedRecipe>
                    </BaseRecipe>
                    <TextJournalEntry Title="""" Contents="""" CreationDate=""100"">
                    </TextJournalEntry>
                    </Journal>";
        [TestMethod]
        public void MisorderedLoadTest()
        {
            // Run the function.
            JournalPersister persister = new JournalPersister();
            Journal createdJournal;
            XDocument doc = XDocument.Parse(MisorderedEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), null, out createdJournal));

            // Check entries
            Assert.AreEqual(1, createdJournal.BaseRecipes.Count());
            Assert.AreEqual(1, createdJournal.Entries.Count());
            Assert.AreEqual(1, createdJournal.Entries.OfType<NoteEntry>().Count());
            Assert.AreEqual(0, createdJournal.Entries.OfType<HomemadeDrinkEntry>().Count());
        }

        private const String InvalidChildEntry = @"<Journal>
                    <BaseRecipe Title="""" Instructions="""">
                    <DerivedRecipe Title="""" Instructions="""" ID=""1"">
                    </DerivedRecipe>
                    </BaseRecipe>
                    <TextJournalEntry Title="""" Contents="""" CreationDate=""100"">
                    </TextJournalEntry>
                    <ModifiedRecipeJournalEntry Title="""" Contents="""" CreationDate=""100"" Rating=""0"" RecipeID=""1"">
                    </ModifiedRecipeJournalEntry>
                    <Blah />
                    </Journal>";
        [TestMethod]
        public void InvalidChildLoadTest()
        {
            // Run the function.
            JournalPersister persister = new JournalPersister();
            Journal createdJournal;
            XDocument doc = XDocument.Parse(InvalidChildEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), null, out createdJournal));

            // Check entries
            Assert.AreEqual(1, createdJournal.BaseRecipes.Count());
            Assert.AreEqual(2, createdJournal.Entries.Count());
            Assert.AreEqual(1, createdJournal.Entries.OfType<NoteEntry>().Count());
            Assert.AreEqual(1, createdJournal.Entries.OfType<HomemadeDrinkEntry>().Count());
            Assert.IsTrue(ReferenceEquals(createdJournal.BaseRecipes.First().DerivedRecipes.First(), createdJournal.Entries.OfType<HomemadeDrinkEntry>().First().Recipe));
        }

        private const String InvalidNameEntry = @"<JournalBlah>
                    <BaseRecipe Title="""" Instructions="""">
                    <DerivedRecipe Title="""" Instructions="""" ID=""1"">
                    </DerivedRecipe>
                    </BaseRecipe>
                    <TextJournalEntry Title="""" Contents="""" CreationDate=""100"">
                    </TextJournalEntry>
                    <ModifiedRecipeJournalEntry Title="""" Contents="""" CreationDate=""100"" Rating=""0"" RecipeID=""1"">
                    </ModifiedRecipeJournalEntry>
                    <Blah />
                    </JournalBlah>";
        [TestMethod]
        public void InvalidNameLoadTest()
        {
            // Run the function.
            JournalPersister persister = new JournalPersister();
            Journal createdJournal;
            XDocument doc = XDocument.Parse(InvalidNameEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), null, out createdJournal));
        }
        #endregion

        #region Saving Tests
        [TestMethod]
        public void BasicSaveTest()
        {
            DateTime time = new DateTime(100);
            Journal journal = new Journal();
            NoteEntry entry = new NoteEntry("", "", time);
            BaseRecipe recipe = new BaseRecipe();
            HomemadeDrink modified = new HomemadeDrink(recipe, 1);
            recipe.AddModifiedRecipe(modified);
            HomemadeDrinkEntry recipeEntry = new HomemadeDrinkEntry(modified, "", time);
            journal.AddEntry(entry);
            journal.AddEntry(recipeEntry);
            journal.AddRecipe(recipe);

            XDocument doc = new XDocument();
            JournalPersister persister = new JournalPersister();
            persister.Write(journal, doc);

            XDocument expectedDoc = XDocument.Parse(BasicEntry);

            CompareXml(expectedDoc, doc);
        }
        #endregion
    }
}
