using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for ModifiedRecipeUnitTests
    /// </summary>
    [TestClass]
    public class ModifiedRecipeUnitTests
    {
        [TestMethod]
        public void NormalConstructorTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            Assert.AreEqual(name, recipe.Name);
            Assert.AreEqual(instructions, recipe.Instructions);
            Assert.IsFalse(recipe.Ingredients.Any());
            Assert.IsTrue(ReferenceEquals(baseRecipe, recipe.BaseRecipe));
            int initialID = recipe.ID;

            HomemadeDrink recipe2 = new HomemadeDrink(baseRecipe);
            Assert.AreEqual(initialID + 1, recipe2.ID);

            HomemadeDrink recipe3 = new HomemadeDrink(baseRecipe, 0);
            Assert.AreEqual(0, recipe3.ID);

            HomemadeDrink recipe4 = new HomemadeDrink(baseRecipe, 2);
            Assert.AreEqual(2, recipe4.ID);
        }

        [TestMethod]
        public void NormalConstructorWithIdTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            int id = 5;
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe, id);
            Assert.AreEqual(name, recipe.Name);
            Assert.AreEqual(instructions, recipe.Instructions);
            Assert.IsFalse(recipe.Ingredients.Any());
            Assert.IsTrue(ReferenceEquals(baseRecipe, recipe.BaseRecipe));
            Assert.AreEqual(id, recipe.ID);

            HomemadeDrink recipe2 = new HomemadeDrink(baseRecipe);
            Assert.IsTrue(id < recipe2.ID);
        }

        [TestMethod]
        public void AddIngredientTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);

            Assert.AreEqual(1, recipe.Ingredients.Count());
            Assert.AreEqual(ingred, recipe.Ingredients.First());
        }

        [TestMethod]
        public void RemoveIngredientTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = ingred.Clone();
            recipe.RemoveIngredient(ingred2);

            Assert.AreEqual(0, recipe.Ingredients.Count());
        }

        [TestMethod]
        public void ClearIngredientsTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe baseRecipe = new BaseRecipe(name, instructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            recipe.ClearIngredients();

            Assert.AreEqual(0, recipe.Ingredients.Count());
        }
    }
}
