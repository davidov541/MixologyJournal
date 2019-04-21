using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class BaseRecipeViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            BaseMixologyApp app = CreateTestApp();
            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(app);

            Assert.AreEqual(0, viewModel.DerivedRecipes.Count());
            Assert.AreEqual(null, viewModel.Favorite);
            Assert.AreEqual(0, viewModel.Ingredients.Count());
            Assert.AreEqual(String.Empty, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(String.Empty, viewModel.Title);
        }

        [TestMethod]
        public void BasicConstructorTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modifiedOne = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modifiedOne);
            recipe.FavoriteRecipe = modifiedOne;
            BaseMixologyApp app = CreateTestApp();

            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);

            Assert.AreEqual(1, viewModel.DerivedRecipes.Count());
            Assert.IsNotNull(viewModel.Favorite);
            Assert.AreEqual(1, viewModel.Ingredients.Count());
            Assert.AreEqual(instructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(name, viewModel.Title);
        }

        [TestMethod]
        public void CloneTest()
        {
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modifiedOne = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modifiedOne);
            BaseMixologyApp app = CreateTestApp();

            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);
            HomemadeDrinkViewModel modifiedViewModel = viewModel.Clone();

            Assert.AreEqual(1, modifiedViewModel.Ingredients.Count());
            Assert.AreEqual(instructions, modifiedViewModel.Instructions);
            Assert.AreEqual(1, modifiedViewModel.ServingsNumber);
            Assert.AreEqual(name, modifiedViewModel.Title);
        }

        [TestMethod]
        public void SaveTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modifiedOne = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modifiedOne);
            recipe.FavoriteRecipe = modifiedOne;
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);

            // Set new values for all of the parts that should be saved.
            String newName = "otherName";
            String newInstructions = "newInstructions";
            viewModel.SetTitle(newName);
            viewModel.SetInstructions(newInstructions);
            viewModel.SetNewFavorite(null, true);
            viewModel.AddModifiedRecipe(viewModel.Clone());
            viewModel.AddIngredient(new EditIngredientViewModel(app));

            // View model should show new values.
            Assert.AreEqual(2, viewModel.DerivedRecipes.Count());
            Assert.AreEqual(null, viewModel.Favorite);
            Assert.AreEqual(2, viewModel.Ingredients.Count());
            Assert.AreEqual(newInstructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(newName, viewModel.Title);

            // Source model should still show old values.
            Assert.AreEqual(1, recipe.DerivedRecipes.Count());
            Assert.AreEqual(modifiedOne, recipe.FavoriteRecipe);
            Assert.AreEqual(1, recipe.Ingredients.Count());
            Assert.AreEqual(instructions, recipe.Instructions);
            Assert.AreEqual(name, recipe.Name);

            viewModel.Save();

            // After save, new values are still in view model.
            Assert.AreEqual(2, viewModel.DerivedRecipes.Count());
            Assert.AreEqual(null, viewModel.Favorite);
            Assert.AreEqual(2, viewModel.Ingredients.Count());
            Assert.AreEqual(newInstructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(newName, viewModel.Title);

            // Source model now has new values as well.
            Assert.AreEqual(2, recipe.DerivedRecipes.Count());
            Assert.AreEqual(null, recipe.FavoriteRecipe);
            Assert.AreEqual(2, recipe.Ingredients.Count());
            Assert.AreEqual(newInstructions, recipe.Instructions);
            Assert.AreEqual(newName, recipe.Name);
        }

        [TestMethod]
        public void CancelTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modifiedOne = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modifiedOne);
            recipe.FavoriteRecipe = modifiedOne;
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);

            // Set new values for all of the parts that should be saved.
            String newName = "otherName";
            String newInstructions = "newInstructions";
            viewModel.SetTitle(newName);
            viewModel.SetInstructions(newInstructions);
            viewModel.SetNewFavorite(null, true);
            viewModel.AddModifiedRecipe(viewModel.Clone());
            viewModel.AddIngredient(new EditIngredientViewModel(app));

            // View model should show new values.
            Assert.AreEqual(2, viewModel.DerivedRecipes.Count());
            Assert.AreEqual(null, viewModel.Favorite);
            Assert.AreEqual(2, viewModel.Ingredients.Count());
            Assert.AreEqual(newInstructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(newName, viewModel.Title);

            // Source model should still show old values.
            Assert.AreEqual(1, recipe.DerivedRecipes.Count());
            Assert.AreEqual(modifiedOne, recipe.FavoriteRecipe);
            Assert.AreEqual(1, recipe.Ingredients.Count());
            Assert.AreEqual(instructions, recipe.Instructions);
            Assert.AreEqual(name, recipe.Name);

            viewModel.Cancel();

            // After cancel, old values are restored to view model.
            Assert.AreEqual(1, viewModel.DerivedRecipes.Count());
            Assert.IsNotNull(viewModel.Favorite);
            Assert.AreEqual(1, viewModel.Ingredients.Count());
            Assert.AreEqual(instructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(name, viewModel.Title);

            // Source model still has old values.
            Assert.AreEqual(1, recipe.DerivedRecipes.Count());
            Assert.AreEqual(modifiedOne, recipe.FavoriteRecipe);
            Assert.AreEqual(1, recipe.Ingredients.Count());
            Assert.AreEqual(instructions, recipe.Instructions);
            Assert.AreEqual(name, recipe.Name);
        }

        [TestMethod]
        public void AddRemoveIngredientsTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modifiedOne = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modifiedOne);
            recipe.FavoriteRecipe = modifiedOne;
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);

            Assert.AreEqual(1, viewModel.Ingredients.Count());

            EditIngredientViewModel ingredOne = new EditIngredientViewModel(app);
            viewModel.AddIngredient(ingredOne);

            Assert.AreEqual(2, viewModel.Ingredients.Count());

            viewModel.RemoveIngredient(ingredOne);

            Assert.AreEqual(1, viewModel.Ingredients.Count());
        }

        [TestMethod]
        public void ServingsNumberPropertyChangedTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);

            bool eventHappened = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                eventHappened = true;
            };
            viewModel.ServingsNumber = 2;
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void FavoritePropertyChangedTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            BaseRecipeViewModel viewModel = new BaseRecipeViewModel(recipe, app);
            HomemadeDrinkViewModel modified = viewModel.Clone();

            bool eventHappened = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                eventHappened = true;
            };
            viewModel.SetNewFavorite(modified, true);
            Assert.IsTrue(eventHappened);
        }
    }
}
