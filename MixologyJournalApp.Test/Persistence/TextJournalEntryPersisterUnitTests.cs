using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;

namespace MixologyJournalApp.Test.Persistence
{
    [TestClass]
    public class TextJournalEntryPersisterUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicEntry = @"<TextJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"">
                  <Picture Path = ""Picture1"" />
                  <Picture Path=""Picture2"" />
                </TextJournalEntry>";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            NoteEntry createdEntry;
            XDocument doc = XDocument.Parse(BasicEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Text Journal", createdEntry.Title);
            Assert.AreEqual("Contents", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);

            // Check Pictures
            Assert.AreEqual(2, createdEntry.Pictures.Count());
            Assert.IsTrue(createdEntry.Pictures.Contains("Picture1"));
            Assert.IsTrue(createdEntry.Pictures.Contains("Picture2"));
        }

        private const String NoPicturesEntry = @"<TextJournalEntry Title=""Text Journal"" Contents=""Contents"" CreationDate=""100"">
                </TextJournalEntry>";
        [TestMethod]
        public void NoPicturesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            NoteEntry createdEntry;
            XDocument doc = XDocument.Parse(NoPicturesEntry);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));

            // Check Entry
            Assert.AreEqual("Text Journal", createdEntry.Title);
            Assert.AreEqual("Contents", createdEntry.Notes);
            Assert.AreEqual(new DateTime(100), createdEntry.CreationDate);

            // Check Pictures
            Assert.AreEqual(0, createdEntry.Pictures.Count());
        }

        private const String NoCreationDateEntry = @"<TextJournalEntry Title=""Text Journal"" Contents=""Contents"">
                </TextJournalEntry>";
        [TestMethod]
        public void NoCreationDateLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            NoteEntry createdEntry;
            XDocument doc = XDocument.Parse(NoCreationDateEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoContentsEntry = @"<TextJournalEntry Title=""Text Journal"" CreationDate=""100"">
                </TextJournalEntry>";
        [TestMethod]
        public void NoContentsLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            NoteEntry createdEntry;
            XDocument doc = XDocument.Parse(NoContentsEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }

        private const String NoTitleEntry = @"<TextJournalEntry Contents=""Contents"" CreationDate=""100"">
                </TextJournalEntry>";
        [TestMethod]
        public void NoTitleLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            NoteEntry createdEntry;
            XDocument doc = XDocument.Parse(NoTitleEntry);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdEntry));
        }
        #endregion

        #region Saving Tests
        [TestMethod]
        public void BasicSaveTest()
        {
            DateTime date = new DateTime(100);
            NoteEntry entry = new NoteEntry("Text Journal", "Contents", date);
            entry.AddPicture("Picture1");
            entry.AddPicture("Picture2");

            XDocument doc = new XDocument();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            persister.Write(entry, doc);

            XDocument expectedDoc = XDocument.Parse(BasicEntry);

            CompareXml(expectedDoc, doc);
        }

        [TestMethod]
        public void NoPicturesSaveTest()
        {
            DateTime date = new DateTime(100);
            NoteEntry entry = new NoteEntry("Text Journal", "Contents", date);

            XDocument doc = new XDocument();
            TextJournalEntryPersister persister = new TextJournalEntryPersister();
            persister.Write(entry, doc);

            XDocument expectedDoc = XDocument.Parse(NoPicturesEntry);

            CompareXml(expectedDoc, doc);
        }
        #endregion
    }
}
