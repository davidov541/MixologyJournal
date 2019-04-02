using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class IngredientViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel vm = new IngredientViewModel(app);

            Assert.AreEqual(new AmountViewModel(new Amount(0.0, AmountUnit.Ounce), app), vm.Amount);
            Assert.AreEqual(null, vm.Description);
            Assert.AreEqual(String.Empty, vm.Details);
            Assert.AreEqual(String.Empty, vm.Name);
            Assert.AreEqual(1.0, vm.ServingsNumber);
        }

        [TestMethod]
        public void NormalConstructorTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            IngredientViewModel vm = new IngredientViewModel(new Ingredient(name, details, amt), app);

            Assert.AreEqual(new AmountViewModel(amt, app), vm.Amount);
            Assert.AreEqual("2 Centiliters of Name", vm.Description);
            Assert.AreEqual(details, vm.Details);
            Assert.AreEqual(name, vm.Name);
            Assert.AreEqual(1.0, vm.ServingsNumber);
        }

        [TestMethod]
        public void CloneTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            IngredientViewModel vm = new IngredientViewModel(new Ingredient(name, details, amt), app);
            IngredientViewModel clone = vm.Clone();

            Assert.AreEqual(new AmountViewModel(amt, app), clone.Amount);
            Assert.AreEqual("2 Centiliters of Name", clone.Description);
            Assert.AreEqual(details, clone.Details);
            Assert.AreEqual(name, clone.Name);
            Assert.AreEqual(1.0, clone.ServingsNumber);
        }

        [TestMethod]
        public void SaveTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            Ingredient model = new Ingredient(name, details, amt);
            IngredientViewModel vm = new IngredientViewModel(model, app);

            Assert.AreEqual(details, vm.Details);
            Assert.AreEqual(name, vm.Name);

            Assert.AreEqual(details, model.Details);
            Assert.AreEqual(name, model.Name);

            String newDetails = "NewDetails";
            String newName = "NewName";
            vm.Details = newDetails;
            vm.Name = newName;

            Assert.AreEqual(newDetails, vm.Details);
            Assert.AreEqual(newName, vm.Name);

            Assert.AreEqual(details, model.Details);
            Assert.AreEqual(name, model.Name);

            vm.Save();

            Assert.AreEqual(newDetails, vm.Details);
            Assert.AreEqual(newName, vm.Name);

            Assert.AreEqual(newDetails, model.Details);
            Assert.AreEqual(newName, model.Name);
        }

        [TestMethod]
        public void CancelTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            Ingredient model = new Ingredient(name, details, amt);
            IngredientViewModel vm = new IngredientViewModel(model, app);

            Assert.AreEqual(details, vm.Details);
            Assert.AreEqual(name, vm.Name);

            Assert.AreEqual(details, model.Details);
            Assert.AreEqual(name, model.Name);

            String newDetails = "NewDetails";
            String newName = "NewName";
            vm.Details = newDetails;
            vm.Name = newName;

            Assert.AreEqual(newDetails, vm.Details);
            Assert.AreEqual(newName, vm.Name);

            Assert.AreEqual(details, model.Details);
            Assert.AreEqual(name, model.Name);

            vm.Cancel();

            Assert.AreEqual(details, vm.Details);
            Assert.AreEqual(name, vm.Name);

            Assert.AreEqual(details, model.Details);
            Assert.AreEqual(name, model.Name);
        }

        [TestMethod]
        public void NameSetTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            Ingredient model = new Ingredient(name, details, amt);
            IngredientViewModel vm = new IngredientViewModel(model, app);

            List<String> propertyChanges = new List<string>();
            vm.PropertyChanged += (sender, e) =>
            {
                propertyChanges.Add(e.PropertyName);
            };

            String newName = "NewName";
            vm.Name = newName;

            Assert.AreEqual(2, propertyChanges.Count);
            Assert.IsTrue(propertyChanges.Contains(nameof(IngredientViewModel.Name)));
            Assert.IsTrue(propertyChanges.Contains(nameof(IngredientViewModel.Description)));
        }

        [TestMethod]
        public void DetailsSetTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            Ingredient model = new Ingredient(name, details, amt);
            IngredientViewModel vm = new IngredientViewModel(model, app);

            List<String> propertyChanges = new List<string>();
            vm.PropertyChanged += (sender, e) =>
            {
                propertyChanges.Add(e.PropertyName);
            };

            vm.Details = "NewDetails";

            Assert.AreEqual(1, propertyChanges.Count);
            Assert.IsTrue(propertyChanges.Contains(nameof(IngredientViewModel.Details)));
        }

        [TestMethod]
        public void ServingsNumberSetTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            Ingredient model = new Ingredient(name, details, amt);
            IngredientViewModel vm = new IngredientViewModel(model, app);

            List<String> propertyChanges = new List<string>();
            vm.PropertyChanged += (sender, e) =>
            {
                propertyChanges.Add(e.PropertyName);
            };

            vm.ServingsNumber = 2;

            Assert.AreEqual(2, propertyChanges.Count);
            Assert.IsTrue(propertyChanges.Contains(nameof(IngredientViewModel.ServingsNumber)));
            Assert.IsTrue(propertyChanges.Contains(nameof(IngredientViewModel.Description)));
        }

        [TestMethod]
        public void AmountValueSetTest()
        {
            IMixologyApp app = CreateTestApp();
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(2.0, AmountUnit.Centiliter);
            Ingredient model = new Ingredient(name, details, amt);
            IngredientViewModel vm = new IngredientViewModel(model, app);

            List<String> propertyChanges = new List<string>();
            vm.PropertyChanged += (sender, e) =>
            {
                propertyChanges.Add(e.PropertyName);
            };

            vm.Amount.Quantity = 5;

            Assert.AreEqual(1, propertyChanges.Count);
            Assert.IsTrue(propertyChanges.Contains(nameof(IngredientViewModel.Description)));
        }
    }
}
