using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;
using Moq;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class TextJournalEntryViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            BaseMixologyApp app = CreateTestApp();
            NotesEntryViewModel viewModel = new NotesEntryViewModel(app);

            Assert.AreEqual(false, viewModel.Busy);
            Assert.AreEqual(viewModel.CreationDate.ToString(), viewModel.Caption);
            Assert.AreEqual(viewModel.Model.CreationDate, viewModel.CreationDate);
            Assert.AreEqual(String.Empty, viewModel.Header);
            Assert.AreEqual(null, viewModel.Icon);
            Assert.AreEqual(String.Empty, viewModel.Notes);
            Assert.AreEqual(0, viewModel.Pictures.Count);
            Assert.AreEqual(String.Empty, viewModel.Title);
        }

        [TestMethod]
        public void NormalConstructorTest()
        {
            BaseMixologyApp app = CreateTestApp();

            String title = "Title";
            String contents = "Conents";
            DateTime creationDate = new DateTime(1000);
            NoteEntry entry = new NoteEntry(title, contents, creationDate);
            NotesEntryViewModel viewModel = new NotesEntryViewModel(entry, app);

            Assert.AreEqual(false, viewModel.Busy);
            Assert.AreEqual(creationDate.ToString(), viewModel.Caption);
            Assert.AreEqual(creationDate, viewModel.CreationDate);
            Assert.AreEqual(title, viewModel.Header);
            Assert.AreEqual(null, viewModel.Icon);
            Assert.AreEqual(contents, viewModel.Notes);
            Assert.AreEqual(0, viewModel.Pictures.Count);
            Assert.AreEqual(title, viewModel.Title);
        }

        [TestMethod]
        public void AddNewPictureSetsBusyTest()
        {
            StartTimingTest();
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel vm = new NotesEntryViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app).Setup(p => p.GetNewPictureAsync()).Returns(GetPictureTaskTest());

            vm.AddNewPictureAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual("Picture", vm.Pictures[0]);
        }

        [TestMethod]
        public void AddExistingPictureSetsBusyTest()
        {
            StartTimingTest();
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel vm = new NotesEntryViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app).Setup(p => p.GetExistingPictureAsync(It.IsAny<UInt64>())).Returns(GetPictureTaskTest());

            vm.AddExistingPictureAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual("Picture", vm.Pictures[0]);
        }

        [TestMethod]
        public void SaveSetsBusyTest()
        {
            StartTimingTest();
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel vm = new NotesEntryViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app.Persister).Setup(p => p.SaveAsync(app.Journal)).Returns(WaitForSaveTest());

            vm.SaveAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
        }

        [TestMethod]
        public void DeleteSetsBusyTest()
        {
            StartTimingTest();
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel vm = new NotesEntryViewModel(app);
            (app.Journal as JournalViewModel).AddEntryAsync(vm).Wait();

            Assert.IsFalse(vm.Busy);

            Mock.Get(app.Persister).Setup(p => p.SaveAsync(app.Journal)).Returns(WaitForSaveTest());

            vm.DeleteAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
        }

        [TestMethod]
        public void SaveTest()
        {
            BaseMixologyApp app = CreateTestApp();

            String title = "Title";
            String contents = "Conents";
            DateTime creationDate = new DateTime(1000);
            NoteEntry entry = new NoteEntry(title, contents, creationDate);
            NotesEntryViewModel viewModel = new NotesEntryViewModel(entry, app);

            String newTitle = "NewTitle";
            String newContents = "NewContents";

            viewModel.Title = newTitle;
            viewModel.Notes = newContents;

            Assert.AreEqual(newContents, viewModel.Notes);
            Assert.AreEqual(newTitle, viewModel.Title);
            Assert.AreEqual(contents, entry.Notes);
            Assert.AreEqual(title, entry.Title);

            viewModel.SaveAsync().Wait();

            Assert.AreEqual(newContents, viewModel.Notes);
            Assert.AreEqual(newTitle, viewModel.Title);
            Assert.AreEqual(newContents, entry.Notes);
            Assert.AreEqual(newTitle, entry.Title);
        }

        [TestMethod]
        public void CancelTest()
        {
            BaseMixologyApp app = CreateTestApp();

            String title = "Title";
            String contents = "Conents";
            DateTime creationDate = new DateTime(1000);
            NoteEntry entry = new NoteEntry(title, contents, creationDate);
            NotesEntryViewModel viewModel = new NotesEntryViewModel(entry, app);

            String newTitle = "NewTitle";
            String newContents = "NewContents";

            viewModel.Title = newTitle;
            viewModel.Notes = newContents;

            Assert.AreEqual(newContents, viewModel.Notes);
            Assert.AreEqual(newTitle, viewModel.Title);
            Assert.AreEqual(contents, entry.Notes);
            Assert.AreEqual(title, entry.Title);

            viewModel.Cancel();

            Assert.AreEqual(contents, viewModel.Notes);
            Assert.AreEqual(title, viewModel.Title);
            Assert.AreEqual(contents, entry.Notes);
            Assert.AreEqual(title, entry.Title);
        }

        [TestMethod]
        public void TitleTest()
        {
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel vm = new NotesEntryViewModel(app);

            bool titleChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(NotesEntryViewModel.Title)))
                {
                    titleChanged = true;
                }
            };

            vm.Title = "NewTitle";

            Assert.IsTrue(titleChanged);
        }

        [TestMethod]
        public void NotesTest()
        {
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel vm = new NotesEntryViewModel(app);

            bool notesChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(NotesEntryViewModel.Notes)))
                {
                    notesChanged = true;
                }
            };

            vm.Notes = "NewTitle";

            Assert.IsTrue(notesChanged);
        }
    }
}
