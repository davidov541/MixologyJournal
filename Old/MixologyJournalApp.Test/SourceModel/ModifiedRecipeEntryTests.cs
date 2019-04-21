using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for ModifiedRecipeUnitTests
    /// </summary>
    [TestClass]
    public class ModifiedRecipeEntryUnitTests
    {
        [TestMethod]
        public void NormalConstructorTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            HomemadeDrinkEntry entry = new HomemadeDrinkEntry(recipe);

            Assert.IsTrue(DateTime.Now.Subtract(entry.CreationDate).TotalMilliseconds < 1000);
            Assert.AreEqual(String.Empty, entry.Notes);
            Assert.AreEqual(0, entry.Pictures.Count());
            Assert.AreEqual(0.0, entry.Rating);
            Assert.AreEqual(recipe, entry.Recipe);
            Assert.AreEqual(recipe.Name, entry.Title);
        }

        [TestMethod]
        public void FullConstructorTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            String contents = "Contents";
            DateTime time = DateTime.FromBinary(1000);
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            HomemadeDrinkEntry entry = new HomemadeDrinkEntry(recipe, contents, time);

            Assert.AreEqual(time, entry.CreationDate);
            Assert.AreEqual(contents, entry.Notes);
            Assert.AreEqual(0, entry.Pictures.Count());
            Assert.AreEqual(0.0, entry.Rating);
            Assert.AreEqual(recipe, entry.Recipe);
            Assert.AreEqual(recipe.Name, entry.Title);
        }

        [TestMethod]
        public void AddPictureTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            HomemadeDrinkEntry entry = new HomemadeDrinkEntry(recipe);

            String picture = "Picture";
            entry.AddPicture(picture);
            Assert.AreEqual(1, entry.Pictures.Count());
            Assert.AreEqual(picture, entry.Pictures.First());
        }
    }
}
