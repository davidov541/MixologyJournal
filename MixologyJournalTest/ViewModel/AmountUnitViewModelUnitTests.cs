using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;
using Moq;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class AmountUnitViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void BasicConstructorTest()
        {
            String nameKey = "Key";
            String nameValue = "Value";
            AmountUnit model = AmountUnit.Drop;
            BaseMixologyApp app = CreateTestApp();
            AmountUnitViewModel uut = new AmountUnitViewModel(nameKey, true, model, app);
            Mock.Get(app).Setup(a => a.GetLocalizedString(nameKey)).Returns(nameValue);

            Assert.AreEqual(nameValue, uut.UserVisibleName);
            Assert.AreEqual(model, uut.Model);
            Assert.IsTrue(uut.QuantityMatters);
        }
    }
}
