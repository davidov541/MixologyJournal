using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Entry;

namespace MixologyJournalApp.Test.SourceModel
{
    /// <summary>
    /// Summary description for ModifiedRecipeUnitTests
    /// </summary>
    [TestClass]
    public class TextJournalEntryUnitTests
    {
        [TestMethod]
        public void FullConstructorTest()
        {
            String name = "Name";
            String contents = "Contents";
            DateTime time = DateTime.FromBinary(1000);
            NoteEntry entry = new NoteEntry(name, contents, time);

            Assert.AreEqual(time, entry.CreationDate);
            Assert.AreEqual(contents, entry.Notes);
            Assert.AreEqual(0, entry.Pictures.Count());
            Assert.AreEqual(name, entry.Title);
        }

        [TestMethod]
        public void AddPictureTest()
        {
            String name = "Name";
            String contents = "Contents";
            DateTime time = DateTime.FromBinary(1000);
            NoteEntry entry = new NoteEntry(name, contents, time);

            String picture = "Picture";
            entry.AddPicture(picture);
            Assert.AreEqual(1, entry.Pictures.Count());
            Assert.AreEqual(picture, entry.Pictures.First());
        }
    }
}
