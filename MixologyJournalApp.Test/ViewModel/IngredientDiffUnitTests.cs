using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class IngredientDiffUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void NoChangeTest()
        {
            IMixologyApp app = CreateTestApp();
            Ingredient ingred = new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce));
            IngredientViewModel ingredOne = new IngredientViewModel(ingred, app);
            IngredientViewModel ingredTwo = new IngredientViewModel(ingred, app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            Assert.AreEqual(ingred.Name, diff.Name);
            Assert.AreEqual(ingred.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Unchanged, diff.Status);
            Assert.AreEqual(AmountDiffStatus.Unchanged, diff.Amount.Status);
        }

        [TestMethod]
        public void AmountsDifferChangeTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);
            IngredientViewModel ingredTwo = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(5.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            Assert.AreEqual(ingredTwo.Name, diff.Name);
            Assert.AreEqual(ingredTwo.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Modified, diff.Status);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Amount.Status);
        }

        [TestMethod]
        public void NamesDifferChangeTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);
            IngredientViewModel ingredTwo = new IngredientViewModel(new Ingredient("Name2", "Brand2", new Amount(5.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            Assert.AreEqual(ingredTwo.Name, diff.Name);
            Assert.AreEqual(ingredTwo.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Replaced, diff.Status);
        }

        [TestMethod]
        public void RemovedTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, null);

            Assert.AreEqual(ingredOne.Name, diff.Name);
            Assert.AreEqual(ingredOne.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Removed, diff.Status);
        }

        [TestMethod]
        public void AddedTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(null, ingredOne);

            Assert.AreEqual(ingredOne.Name, diff.Name);
            Assert.AreEqual(ingredOne.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Added, diff.Status);
        }

        [TestMethod]
        public void SetNameTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);
            IngredientViewModel ingredTwo = new IngredientViewModel(new Ingredient("Name2", "Brand2", new Amount(5.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            bool nameChangeSeen = false;
            bool statusChangeSeen = false;
            diff.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(IngredientDiff.Name)))
                {
                    nameChangeSeen = true;
                }
                else if (e.PropertyName.Equals(nameof(IngredientDiff.Status)))
                {
                    statusChangeSeen = true;
                }
                else
                {
                    Assert.Fail("Unexpected property changed!");
                }
            };

            String newName = "New Name";
            diff.Name = newName;

            Assert.IsTrue(nameChangeSeen);
            Assert.IsTrue(statusChangeSeen);
            Assert.AreEqual(newName, ingredTwo.Name);
        }

        [TestMethod]
        public void SetNameRemovedTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, null);

            bool nameChangeSeen = false;
            bool statusChangeSeen = false;
            diff.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(IngredientDiff.Name)))
                {
                    nameChangeSeen = true;
                }
                else if (e.PropertyName.Equals(nameof(IngredientDiff.Status)))
                {
                    statusChangeSeen = true;
                }
                else
                {
                    Assert.Fail("Unexpected property changed!");
                }
            };

            String newName = "New Name";
            diff.Name = newName;

            Assert.IsTrue(nameChangeSeen);
            Assert.IsTrue(statusChangeSeen);
            Assert.AreEqual("Name", ingredOne.Name);
        }

        [TestMethod]
        public void SetBrandTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);
            IngredientViewModel ingredTwo = new IngredientViewModel(new Ingredient("Name2", "Brand2", new Amount(5.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            bool detailsChangeSeen = false;
            diff.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(IngredientDiff.Details)))
                {
                    detailsChangeSeen = true;
                }
                else
                {
                    Assert.Fail("Unexpected property changed!");
                }
            };

            String newDetails = "New Brand";
            diff.Details = newDetails;

            Assert.IsTrue(detailsChangeSeen);
            Assert.AreEqual(ingredTwo.Details, newDetails);
        }

        [TestMethod]
        public void SetBrandRemovedTest()
        {
            IMixologyApp app = CreateTestApp();
            IngredientViewModel ingredOne = new IngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, null);

            bool detailsChangeSeen = false;
            diff.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(IngredientDiff.Details)))
                {
                    detailsChangeSeen = true;
                }
                else
                {
                    Assert.Fail("Unexpected property changed!");
                }
            };

            String newDetails = "New Brand";
            diff.Details = newDetails;

            Assert.IsTrue(detailsChangeSeen);
            Assert.AreEqual(ingredOne.Details, "Brand");
        }

    }
}
