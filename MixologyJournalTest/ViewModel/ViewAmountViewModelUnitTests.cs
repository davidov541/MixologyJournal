using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class ViewAmountViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void NormalConstructorTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            ViewAmountViewModel viewModel = new ViewAmountViewModel(model, app);

            Assert.AreEqual(2, viewModel.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (viewModel.Unit as AmountUnitViewModel).Model);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual("2", viewModel.Description);
        }

        [TestMethod]
        public void ServingsNumberTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            ViewAmountViewModel viewModel = new ViewAmountViewModel(model, app);

            viewModel.ServingsNumber = 2;

            Assert.AreEqual(2, viewModel.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (viewModel.Unit as AmountUnitViewModel).Model);
            Assert.AreEqual(2, viewModel.ServingsNumber);
            Assert.AreEqual("4", viewModel.Description);
        }

        [TestMethod]
        public void SetServingNumberTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            ViewAmountViewModel viewModel = new ViewAmountViewModel(model, app);

            PropertyChangeTest(viewModel, () => viewModel.ServingsNumber = 3, new List<String> { "Description", "ServingsNumber" });
            Assert.AreEqual(3, viewModel.ServingsNumber);
        }

        private void PropertyChangeTest(ViewAmountViewModel viewModel, Action change, IEnumerable<String> expectedNames)
        {
            List<String> names = new List<string>(expectedNames);
            viewModel.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(names.Contains(e.PropertyName));
                names.Remove(e.PropertyName);
            };
            change();
            Assert.AreEqual(0, names.Count);
        }
    }
}
