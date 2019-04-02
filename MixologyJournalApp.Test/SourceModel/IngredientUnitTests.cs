using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for IngredientUnitTests
    /// </summary>
    [TestClass]
    public class IngredientUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Ingredient ingred = new Ingredient();
            Assert.AreEqual(String.Empty, ingred.Name);
            Assert.AreEqual(String.Empty, ingred.Details);
            Assert.AreEqual(new Amount(), ingred.Amount);
        }

        [TestMethod]
        public void NormalConstructorTest()
        {
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(5.0, AmountUnit.Piece);
            Ingredient ingred = new Ingredient(name, details, amt);
            Assert.AreEqual(name, ingred.Name);
            Assert.AreEqual(details, ingred.Details);
            Assert.AreEqual(amt, ingred.Amount);
        }

        [TestMethod]
        public void ClonerTest()
        {
            String name = "Name";
            String details = "Details";
            Amount amt = new Amount(5.0, AmountUnit.Piece);
            Ingredient ingred = new Ingredient(name, details, amt);
            Ingredient cloned = ingred.Clone();

            Assert.AreEqual(name, cloned.Name);
            Assert.AreEqual(details, cloned.Details);
            Assert.AreEqual(amt, cloned.Amount);
        }

    }
}
