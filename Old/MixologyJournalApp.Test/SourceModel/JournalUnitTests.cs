using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using Moq;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for JournalUnitTess
    /// </summary>
    [TestClass]
    public class JournalUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Journal journal = new Journal();
            Assert.AreEqual(0, journal.BaseRecipes.Count());
            Assert.AreEqual(0, journal.Entries.Count());
        }

        [TestMethod]
        public void AddEntryTest()
        {
            Journal journal = new Journal();
            Mock<IJournalEntry> mock = new Mock<IJournalEntry>();
            journal.AddEntry(mock.Object);

            Assert.AreEqual(1, journal.Entries.Count());
            Assert.AreEqual(mock.Object, journal.Entries.First());
        }

        [TestMethod]
        public void RemoveEntryTest()
        {
            Journal journal = new Journal();
            Mock<IJournalEntry> mock = new Mock<IJournalEntry>();
            journal.AddEntry(mock.Object);
            journal.RemoveEntry(mock.Object);

            Assert.AreEqual(0, journal.Entries.Count());
        }

        [TestMethod]
        public void AddRecipeTest()
        {
            Journal journal = new Journal();
            BaseRecipe recipe = new BaseRecipe();
            journal.AddRecipe(recipe);

            Assert.AreEqual(1, journal.BaseRecipes.Count());
            Assert.AreEqual(recipe, journal.BaseRecipes.First());
        }

        [TestMethod]
        public void GetModifedRecipeTest()
        {
            Journal journal = new Journal();
            BaseRecipe recipe = new BaseRecipe();
            int id = 10;
            HomemadeDrink modified = new HomemadeDrink(recipe, id);
            recipe.AddModifiedRecipe(modified);
            journal.AddRecipe(recipe);

            Assert.AreEqual(modified, journal.GetModifiedRecipe(id));
        }

        [TestMethod]
        public void RemoveRecipeTest()
        {
            Journal journal = new Journal();
            BaseRecipe recipe = new BaseRecipe();
            journal.AddRecipe(recipe);
            journal.RemoveRecipe(recipe);

            Assert.AreEqual(0, journal.BaseRecipes.Count());
        }
    }
}
