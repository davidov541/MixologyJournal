using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.Test.SourceModel
{
    /// <summary>
    /// Summary description for BarDrinkEntryUnitTests
    /// </summary>
    [TestClass]
    public class BarDrinkEntryUnitTests
    {
        [TestMethod]
        public void NormalConstructorTest()
        {
            BarDrink drink = new BarDrink();
            BarDrinkEntry entry = new BarDrinkEntry(drink);

            Assert.IsTrue(DateTime.Now.Subtract(entry.CreationDate).TotalMilliseconds < 1000);
            Assert.AreEqual(String.Empty, entry.Notes);
            Assert.AreEqual(0, entry.Pictures.Count());
            Assert.AreEqual(0.0, entry.Rating);
            Assert.AreEqual(drink, entry.Recipe);
            Assert.AreEqual(drink.Name, entry.Title);
        }

        [TestMethod]
        public void FullConstructorTest()
        {
            String contents = "Contents";
            DateTime time = DateTime.FromBinary(1000);
            BarDrink drink = new BarDrink();
            BarDrinkEntry entry = new BarDrinkEntry(drink, contents, time);

            Assert.AreEqual(time, entry.CreationDate);
            Assert.AreEqual(contents, entry.Notes);
            Assert.AreEqual(0, entry.Pictures.Count());
            Assert.AreEqual(0.0, entry.Rating);
            Assert.AreEqual(drink, entry.Recipe);
            Assert.AreEqual(drink.Name, entry.Title);
        }

        [TestMethod]
        public void AddPictureTest()
        {
            BarDrink drink = new BarDrink();
            BarDrinkEntry entry = new BarDrinkEntry(drink);

            String picture = "Picture";
            entry.AddPicture(picture);
            Assert.AreEqual(1, entry.Pictures.Count());
            Assert.AreEqual(picture, entry.Pictures.First());
        }
    }
}
