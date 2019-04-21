using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class ModifiedRecipeViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void BasicConstructorTest()
        {
            BaseMixologyApp app = CreateTestApp();

            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);

            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            Assert.AreEqual(1, viewModel.Ingredients.Count());
            Assert.AreEqual(instructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(name, viewModel.Title);
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
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            // Set new values for all of the parts that should be saved.
            String newName = "otherName";
            String newInstructions = "newInstructions";
            viewModel.SetTitle(newName);
            viewModel.SetInstructions(newInstructions);
            viewModel.AddIngredient(new EditIngredientViewModel(app));

            // View model should show new values.
            Assert.AreEqual(2, viewModel.Ingredients.Count());
            Assert.AreEqual(newInstructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(newName, viewModel.Title);

            // Source model should still show old values.
            Assert.AreEqual(1, modified.Ingredients.Count());
            Assert.AreEqual(instructions, modified.Instructions);
            Assert.AreEqual(name, modified.Name);

            viewModel.Save();

            // After save, new values are still in view model.
            Assert.AreEqual(2, viewModel.Ingredients.Count());
            Assert.AreEqual(newInstructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(newName, viewModel.Title);

            // Source model now has new values as well.
            Assert.AreEqual(2, modified.Ingredients.Count());
            Assert.AreEqual(newInstructions, modified.Instructions);
            Assert.AreEqual(newName, modified.Name);
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
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            // Set new values for all of the parts that should be saved.
            String newName = "otherName";
            String newInstructions = "newInstructions";
            viewModel.SetTitle(newName);
            viewModel.SetInstructions(newInstructions);
            viewModel.AddIngredient(new EditIngredientViewModel(app));

            // View model should show new values.
            Assert.AreEqual(2, viewModel.Ingredients.Count());
            Assert.AreEqual(newInstructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(newName, viewModel.Title);

            // Source model should still show old values.
            Assert.AreEqual(1, modified.Ingredients.Count());
            Assert.AreEqual(instructions, modified.Instructions);
            Assert.AreEqual(name, modified.Name);

            viewModel.Cancel();

            // After cancel, old values are restored to view model.
            Assert.AreEqual(1, viewModel.Ingredients.Count());
            Assert.AreEqual(instructions, viewModel.Instructions);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual(name, viewModel.Title);

            // Source model still has old values.
            Assert.AreEqual(1, modified.Ingredients.Count());
            Assert.AreEqual(instructions, modified.Instructions);
            Assert.AreEqual(name, modified.Name);
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
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

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
            Ingredient ingred = new Ingredient("Ingred", "Brand", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            bool eventHappened = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                eventHappened = true;
            };
            viewModel.ServingsNumber = 2;
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void CaptionNoChangesTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            Assert.AreEqual("No Changes", viewModel.Caption);
        }

        [TestMethod]
        public void CaptionDetailsChangedTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred2", "", new Amount(2, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            Ingredient ingred3 = new Ingredient("Ingred3", "", new Amount(2, AmountUnit.Liter));
            recipe.AddIngredient(ingred3);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            modified.Ingredients.ElementAt(0).Details = "Details1";
            modified.Ingredients.ElementAt(1).Details = "Details2";
            modified.Ingredients.ElementAt(2).Details = "Details3";
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            Assert.AreEqual("With Details1, Details2, & Details3", viewModel.Caption);
        }

        [TestMethod]
        public void ChangedAmountChangedTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred2", "", new Amount(2, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            Ingredient ingred3 = new Ingredient("Ingred3", "", new Amount(2, AmountUnit.Liter));
            recipe.AddIngredient(ingred3);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            modified.Ingredients.ElementAt(0).Amount.Quantity = 3;
            modified.Ingredients.ElementAt(1).Amount.Quantity = 3;
            modified.Ingredients.ElementAt(2).Amount.Quantity = 3;
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            Assert.AreEqual("Less Ingred, More Ingred2, & More Ingred3", viewModel.Caption);
        }

        [TestMethod]
        public void AddedRemovedTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred2", "", new Amount(2, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            modified.RemoveIngredient(modified.Ingredients.First());
            Ingredient ingred3 = new Ingredient("Ingred3", "", new Amount(2, AmountUnit.Liter));
            modified.AddIngredient(ingred3);
            Ingredient ingred4 = new Ingredient("Ingred4", "", new Amount(2, AmountUnit.Liter));
            modified.AddIngredient(ingred4);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            Assert.AreEqual("Added Ingred3, Added Ingred4, & Removed Ingred", viewModel.Caption);
        }

        [TestMethod]
        public void AllThreeOrderOfPriorityTest()
        {
            // Set up model.
            String name = "Name";
            String instructions = "Instructions";
            BaseRecipe recipe = new BaseRecipe(name, instructions);
            Ingredient ingred = new Ingredient("Ingred", "", new Amount(5, AmountUnit.Liter));
            recipe.AddIngredient(ingred);
            Ingredient ingred2 = new Ingredient("Ingred2", "", new Amount(2, AmountUnit.Liter));
            recipe.AddIngredient(ingred2);
            HomemadeDrink modified = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modified);
            modified.RemoveIngredient(modified.Ingredients.First());
            modified.Ingredients.First().Details = "Details1";
            modified.Ingredients.Last().Amount.Quantity = 4;
            Ingredient ingred3 = new Ingredient("Ingred3", "", new Amount(2, AmountUnit.Liter));
            modified.AddIngredient(ingred3);
            Ingredient ingred4 = new Ingredient("Ingred4", "", new Amount(2, AmountUnit.Liter));
            modified.AddIngredient(ingred4);
            BaseMixologyApp app = CreateTestApp();

            // Create UUT
            HomemadeDrinkViewModel viewModel = new HomemadeDrinkViewModel(modified, new BaseRecipeViewModel(recipe, app), app);

            Assert.AreEqual("Added Ingred3, Added Ingred4, & Removed Ingred", viewModel.Caption);
        }
    }
}
