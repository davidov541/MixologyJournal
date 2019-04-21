using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for BaseRecipeUnitTests
    /// </summary>
    [TestClass]
    public class BaseRecipeUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            BaseRecipe recipe = new BaseRecipe();
            Assert.AreEqual(String.Empty, recipe.Name);
            Assert.AreEqual(String.Empty, recipe.Instructions);
            Assert.IsNull(recipe.FavoriteRecipe);
            Assert.IsFalse(recipe.DerivedRecipes.Any());
            Assert.IsFalse(recipe.Ingredients.Any());
        }

        [TestMethod]
        public void NormalConstructorTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Assert.AreEqual(name, recipe.Name);
            Assert.AreEqual(instructions, recipe.Instructions);
            Assert.IsNull(recipe.FavoriteRecipe);
            Assert.IsFalse(recipe.DerivedRecipes.Any());
            Assert.IsFalse(recipe.Ingredients.Any());
        }

        [TestMethod]
        public void AddModifiedRecipe()
        {
            BaseRecipe recipe = new BaseRecipe();
            HomemadeDrink modified = new HomemadeDrink(recipe, 1);
            recipe.AddModifiedRecipe(modified);
            Assert.AreEqual(1, recipe.DerivedRecipes.Count());
            Assert.IsTrue(ReferenceEquals(modified, recipe.DerivedRecipes.First()));
        }

        [TestMethod]
        public void CloneTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);

            HomemadeDrink cloned = recipe.Clone();
            Assert.AreEqual(name, cloned.Name);
            Assert.AreEqual(instructions, cloned.Instructions);
            Assert.IsTrue(ReferenceEquals(recipe, cloned.BaseRecipe));
            Assert.AreEqual(1, cloned.Ingredients.Count());
            Assert.AreEqual(ingred, cloned.Ingredients.First());
        }

        [TestMethod]
        public void AddIngredientTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
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
            BaseRecipe recipe = new BaseRecipe(name, instructions);
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
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            recipe.ClearIngredients();

            Assert.AreEqual(0, recipe.Ingredients.Count());
        }
    }
}
