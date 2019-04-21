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
            BaseMixologyApp app = CreateTestApp();
            Ingredient ingred = new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce));
            EditIngredientViewModel ingredOne = new EditIngredientViewModel(ingred, app);
            EditIngredientViewModel ingredTwo = new EditIngredientViewModel(ingred, app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            Assert.AreEqual(ingred.Name, diff.Name);
            Assert.AreEqual(ingred.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Unchanged, diff.Status);
            Assert.AreEqual(AmountDiffStatus.Unchanged, diff.Amount.Status);
        }

        [TestMethod]
        public void AmountsDifferChangeTest()
        {
            BaseMixologyApp app = CreateTestApp();
            EditIngredientViewModel ingredOne = new EditIngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);
            EditIngredientViewModel ingredTwo = new EditIngredientViewModel(new Ingredient("Name", "Brand", new Amount(5.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            Assert.AreEqual(ingredTwo.Name, diff.Name);
            Assert.AreEqual(ingredTwo.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Modified, diff.Status);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Amount.Status);
        }

        [TestMethod]
        public void NamesDifferChangeTest()
        {
            BaseMixologyApp app = CreateTestApp();
            EditIngredientViewModel ingredOne = new EditIngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);
            EditIngredientViewModel ingredTwo = new EditIngredientViewModel(new Ingredient("Name2", "Brand2", new Amount(5.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, ingredTwo);

            Assert.AreEqual(ingredTwo.Name, diff.Name);
            Assert.AreEqual(ingredTwo.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Replaced, diff.Status);
        }

        [TestMethod]
        public void RemovedTest()
        {
            BaseMixologyApp app = CreateTestApp();
            EditIngredientViewModel ingredOne = new EditIngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(ingredOne, null);

            Assert.AreEqual(ingredOne.Name, diff.Name);
            Assert.AreEqual(ingredOne.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Removed, diff.Status);
        }

        [TestMethod]
        public void AddedTest()
        {
            BaseMixologyApp app = CreateTestApp();
            EditIngredientViewModel ingredOne = new EditIngredientViewModel(new Ingredient("Name", "Brand", new Amount(2.0, AmountUnit.Ounce)), app);

            IngredientDiff diff = new IngredientDiff(null, ingredOne);

            Assert.AreEqual(ingredOne.Name, diff.Name);
            Assert.AreEqual(ingredOne.Details, diff.Details);
            Assert.AreEqual(ChangedStatus.Added, diff.Status);
        }
    }
}
