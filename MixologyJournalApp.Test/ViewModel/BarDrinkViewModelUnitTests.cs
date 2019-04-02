using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class BarDrinkViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void BasicConstructorTest()
        {
            IMixologyApp app = CreateTestApp();

            BarDrinkViewModel vm = new BarDrinkViewModel(app);

            Assert.AreEqual(0, vm.Ingredients.Count);
            Assert.AreEqual(String.Empty, vm.Instructions);
            Assert.AreEqual(1, vm.ServingsNumber);
            Assert.AreEqual(String.Empty, vm.Title);
        }

        [TestMethod]
        public void SaveTest()
        {
            // Set up model.
            IMixologyApp app = CreateTestApp();

            // Create UUT
            BarDrinkViewModel vm = new BarDrinkViewModel(app);

            // Set new values for all of the parts that should be saved.
            String newName = "otherName";
            String newInstructions = "newInstructions";
            vm.Title = newName;
            vm.Instructions = newInstructions;
            vm.AddIngredient(new IngredientViewModel(app));

            // View model should show new values.
            Assert.AreEqual(1, vm.Ingredients.Count);
            Assert.AreEqual(newInstructions, vm.Instructions);
            Assert.AreEqual(1, vm.ServingsNumber);
            Assert.AreEqual(newName, vm.Title);

            // Source model should still show old values.
            Assert.AreEqual(0, vm.Model.Ingredients.Count());
            Assert.AreEqual(String.Empty, vm.Model.Instructions);
            Assert.AreEqual(String.Empty, vm.Model.Name);

            vm.Save();

            // After save, new values are still in view model.
            Assert.AreEqual(1, vm.Ingredients.Count);
            Assert.AreEqual(newInstructions, vm.Instructions);
            Assert.AreEqual(1, vm.ServingsNumber);
            Assert.AreEqual(newName, vm.Title);

            // Source model now has new values as well.
            Assert.AreEqual(1, vm.Model.Ingredients.Count());
            Assert.AreEqual(newInstructions, vm.Model.Instructions);
            Assert.AreEqual(newName, vm.Model.Name);
        }

        [TestMethod]
        public void CancelTest()
        {
            // Set up model.
            IMixologyApp app = CreateTestApp();

            // Create UUT
            BarDrinkViewModel vm = new BarDrinkViewModel(app);

            // Set new values for all of the parts that should be saved.
            String newName = "otherName";
            String newInstructions = "newInstructions";
            vm.Title = newName;
            vm.Instructions = newInstructions;
            vm.AddIngredient(new IngredientViewModel(app));

            // View model should show new values.
            Assert.AreEqual(1, vm.Ingredients.Count);
            Assert.AreEqual(newInstructions, vm.Instructions);
            Assert.AreEqual(1, vm.ServingsNumber);
            Assert.AreEqual(newName, vm.Title);

            // Source model should still show old values.
            Assert.AreEqual(0, vm.Model.Ingredients.Count());
            Assert.AreEqual(String.Empty, vm.Model.Instructions);
            Assert.AreEqual(String.Empty, vm.Model.Name);

            vm.Cancel();

            // After cancel, old values are restored to view model.
            Assert.AreEqual(0, vm.Ingredients.Count);
            Assert.AreEqual(String.Empty, vm.Instructions);
            Assert.AreEqual(1, vm.ServingsNumber);
            Assert.AreEqual(String.Empty, vm.Title);

            // Source model still has old values.
            Assert.AreEqual(0, vm.Model.Ingredients.Count());
            Assert.AreEqual(String.Empty, vm.Model.Instructions);
            Assert.AreEqual(String.Empty, vm.Model.Name);
        }

        [TestMethod]
        public void AddRemoveIngredientsTest()
        {
            // Set up model.
            IMixologyApp app = CreateTestApp();

            // Create UUT
            BarDrinkViewModel vm = new BarDrinkViewModel(app);

            Assert.AreEqual(0, vm.Ingredients.Count);

            IngredientViewModel ingredOne = new IngredientViewModel(app);
            vm.AddIngredient(ingredOne);

            Assert.AreEqual(1, vm.Ingredients.Count);

            vm.RemoveIngredient(ingredOne);

            Assert.AreEqual(0, vm.Ingredients.Count);
        }

        [TestMethod]
        public void ServingsNumberPropertyChangedTest()
        {
            // Set up model.
            IMixologyApp app = CreateTestApp();

            // Create UUT
            BarDrinkViewModel vm = new BarDrinkViewModel(app);

            bool eventHappened = false;
            vm.PropertyChanged += (sender, e) =>
            {
                eventHappened = true;
            };
            vm.ServingsNumber = 2;
            Assert.IsTrue(eventHappened);
        }
    }
}
