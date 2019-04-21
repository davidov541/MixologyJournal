using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
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
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(4, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void DecreasedAmountTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(1, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Decreased, diff.Status);
            Assert.AreEqual(1, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void SameAmountTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Unchanged, diff.Status);
            Assert.AreEqual(2, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void DifferentUnitsTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(4, AmountUnit.Liter), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Liter, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void ModifiedPiecesTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(4, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.ChangedButUnknown, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void OriginalPiecesTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(4, AmountUnit.Piece), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(2, AmountUnit.Ounce), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.ChangedButUnknown, diff.Status);
            Assert.AreEqual(2, diff.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (diff.Unit as AmountUnitViewModel).Model);
        }
        [TestMethod]
        public void IncreasedAmountPiecesTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Piece), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(4, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Increased, diff.Status);
            Assert.AreEqual(4, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void DecreasedAmountPiecesTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Piece), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(1, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Decreased, diff.Status);
            Assert.AreEqual(1, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void SameAmountPiecesTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Piece), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(2, AmountUnit.Piece), app);
            AmountDiff diff = new AmountDiff(original, modified);
            Assert.AreEqual(AmountDiffStatus.Unchanged, diff.Status);
            Assert.AreEqual(2, diff.Quantity);
            Assert.AreEqual(AmountUnit.Piece, (diff.Unit as AmountUnitViewModel).Model);
        }

        [TestMethod]
        public void UnitSetterTest()
        {
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Piece), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(2, AmountUnit.Piece), app);
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
            BaseMixologyApp app = CreateTestApp();
            ViewAmountViewModel original = new ViewAmountViewModel(new Amount(2, AmountUnit.Piece), app);
            EditAmountViewModel modified = new EditAmountViewModel(new Amount(2, AmountUnit.Piece), app);
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
