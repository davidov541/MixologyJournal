using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.SourceModel
{
    [TestClass]
    public class AmountUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Amount amt = new Amount();
            Assert.AreEqual(0, amt.Quantity);
            Assert.AreEqual(AmountUnit.Ounce, amt.Unit);
        }

        [TestMethod]
        public void NormalConstructorTest()
        {
            Amount amt = new Amount(5.0, AmountUnit.Pint);
            Assert.AreEqual(5.0, amt.Quantity);
            Assert.AreEqual(AmountUnit.Pint, amt.Unit);
        }

        [TestMethod]
        public void CloneTest()
        {
            Amount amt = new Amount(5.0, AmountUnit.Pint);
            Amount cloned = amt.Clone();
            Assert.AreEqual(5.0, cloned.Quantity);
            Assert.AreEqual(AmountUnit.Pint, cloned.Unit);
        }
    }
}
