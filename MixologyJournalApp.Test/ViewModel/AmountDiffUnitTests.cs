using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class AmountDiffUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void IncreasedAmountTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountViewModel modified = new AmountViewModel(new Amount(4, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void DecreasedAmountTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountViewModel modified = new AmountViewModel(new Amount(1, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Decreased, diff.Status);
            Assert.AreEqual(1, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void SameAmountTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountViewModel modified = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Unchanged, diff.Status);
            Assert.AreEqual(2, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void DifferentUnitsTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountViewModel modified = new AmountViewModel(new Amount(4, AmountUnit.Liter), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Liter, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void ModifiedPiecesTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountViewModel modified = new AmountViewModel(new Amount(4, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.ChangedButUnknown, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void OriginalPiecesTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(4, AmountUnit.Piece), app);
            AmountViewModel modified = new AmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.ChangedButUnknown, diff.Status);
            Assert.AreEqual(2, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }
        [TestMethod]
        public void IncreasedAmountPiecesTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountViewModel modified = new AmountViewModel(new Amount(4, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void DecreasedAmountPiecesTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountViewModel modified = new AmountViewModel(new Amount(1, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Decreased, diff.Status);
            Assert.AreEqual(1, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void SameAmountPiecesTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountViewModel modified = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Unchanged, diff.Status);
            Assert.AreEqual(2, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void UnitSetterTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountViewModel modified = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);

            bool eventCameIn = false;
            diff.PropertyChanged += (e, sender) =>
            {
                eventCameIn = true;
            };
            AmountUnitViewModel newUnit = new AmountUnitViewModel("Drop", true, AmountUnit.Drop, app);
            diff.Unit = newUnit;
            Assert.IsTrue(eventCameIn);
            Assert.AreEqual(newUnit, modified.Unit);
        }

        [TestMethod]
        public void QuantitySetterTest()
        {
            IMixologyApp app = CreateTestApp();
            AmountViewModel original = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountViewModel modified = new AmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);

            bool eventCameIn = false;
            diff.PropertyChanged += (e, sender) =>
            {
                eventCameIn = true;
            };
            diff.Quantity = 4.0;
            Assert.IsTrue(eventCameIn);
            Assert.AreEqual(4.0, modified.Quantity);
        }
    }
}
