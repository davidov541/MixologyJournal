using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class EditAmountViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void NormalConstructorTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            Assert.AreEqual(2, viewModel.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (viewModel.Unit as AmountUnitViewModel).Model);
            Assert.AreEqual(1, viewModel.ServingsNumber);
            Assert.AreEqual("2", viewModel.Description);
        }

        [TestMethod]
        public void SaveTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            viewModel.Quantity = 4;
            AmountUnitViewModel chosenUnit = viewModel.AvailableUnits.ElementAt(2) as AmountUnitViewModel;
            viewModel.Unit = chosenUnit;
            Assert.AreEqual(2, model.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, model.Unit);
            Assert.AreEqual(4, viewModel.Quantity);
            Assert.AreEqual(chosenUnit, viewModel.Unit);

            viewModel.Save();
            Assert.AreEqual(4, model.Quantity);
            Assert.AreEqual(chosenUnit.Model, model.Unit);
            Assert.AreEqual(4, viewModel.Quantity);
            Assert.AreEqual(chosenUnit, viewModel.Unit);
        }

        [TestMethod]
        public void CancelTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            viewModel.Quantity = 4;
            AmountUnitViewModel chosenUnit = viewModel.AvailableUnits.ElementAt(2) as AmountUnitViewModel;
            viewModel.Unit = chosenUnit;
            Assert.AreEqual(2, model.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, model.Unit);
            Assert.AreEqual(4, viewModel.Quantity);
            Assert.AreEqual(chosenUnit, viewModel.Unit);

            viewModel.Cancel();
            Assert.AreEqual(2, model.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, model.Unit);
            Assert.AreEqual(2, viewModel.Quantity);
            Assert.AreEqual(viewModel.AvailableUnits.OfType<AmountUnitViewModel>().First(u => u.Model == AmountUnit.Ounce), viewModel.Unit);
        }

        [TestMethod]
        public void ServingsNumberTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            viewModel.ServingsNumber = 2;

            Assert.AreEqual(2, viewModel.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, (viewModel.Unit as AmountUnitViewModel).Model);
            Assert.AreEqual(2, viewModel.ServingsNumber);
            Assert.AreEqual("4", viewModel.Description);
        }

        [TestMethod]
        public void SetQuantityTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            PropertyChangeTest(viewModel, () => viewModel.Quantity = 4, new List<String> { "Description", "Quantity" });
            Assert.AreEqual(4, viewModel.Quantity);
        }

        [TestMethod]
        public void SetUnitTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            AmountUnitViewModel nextUnit = viewModel.AvailableUnits.OfType<AmountUnitViewModel>().First(u => u.Model == AmountUnit.Cup);
            PropertyChangeTest(viewModel, () => viewModel.Unit = nextUnit , new List<String> { "Description", "Unit" });
            Assert.AreEqual(nextUnit, viewModel.Unit);
        }

        [TestMethod]
        public void SetServingNumberTest()
        {
            BaseMixologyApp app = CreateTestApp();
            Amount model = new Amount(2, AmountUnit.Ounce);
            EditAmountViewModel viewModel = new EditAmountViewModel(model, app);

            PropertyChangeTest(viewModel, () => viewModel.ServingsNumber = 3, new List<String> { "Description", "ServingsNumber" });
            Assert.AreEqual(3, viewModel.ServingsNumber);
        }

        private void PropertyChangeTest(EditAmountViewModel viewModel, Action change, IEnumerable<String> expectedNames)
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
