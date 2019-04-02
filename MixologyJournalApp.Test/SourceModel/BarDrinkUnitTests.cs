using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for BarDrinkUnitTests
    /// </summary>
    [TestClass]
    public class BarDrinkUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            BarDrink recipe = new BarDrink();
            Assert.AreEqual(String.Empty, recipe.Name);
            Assert.AreEqual(String.Empty, recipe.Instructions);
            Assert.IsFalse(recipe.Ingredients.Any());
        }

        [TestMethod]
        public void IdConstructorTest()
        {
            int id = 5;
            BarDrink recipe = new BarDrink(id);
            Assert.AreEqual(String.Empty, recipe.Name);
            Assert.AreEqual(String.Empty, recipe.Instructions);
            Assert.AreEqual(5, recipe.ID);
            Assert.IsFalse(recipe.Ingredients.Any());

            BarDrink recipe2 = new BarDrink();
            Assert.IsTrue(id < recipe2.ID);
        }

        [TestMethod]
        public void AddIngredientTest()
        {
            BarDrink recipe = new BarDrink();
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);

            Assert.AreEqual(1, recipe.Ingredients.Count());
            Assert.AreEqual(ingred, recipe.Ingredients.First());
        }

        [TestMethod]
        public void RemoveIngredientTest()
        {
            BarDrink recipe = new BarDrink();
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = ingred.Clone();
            recipe.RemoveIngredient(ingred2);

            Assert.AreEqual(0, recipe.Ingredients.Count());
        }

        [TestMethod]
        public void ClearIngredientsTest()
        {
            BarDrink recipe = new BarDrink();
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            recipe.ClearIngredients();

            Assert.AreEqual(0, recipe.Ingredients.Count());
        }
    }
}
