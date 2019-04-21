using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using Moq;

namespace MixologyJournalApp.Test.Persistence
{
    [TestClass]
    public class AppPersisterUnitTests
    {
        private Mock<IPersistenceSource> _localSource;
        private Mock<IPersistenceSource> _cloudSource;

        [TestMethod]
        public void AddSourceTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);

            Assert.AreEqual(2, persister.Sources.Count());
            Assert.IsTrue(persister.Sources.Contains(_localSource.Object));
            Assert.IsTrue(persister.Sources.Contains(_cloudSource.Object));
        }

        [TestMethod]
        public void SaveAsyncSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;
            app.Setup(a => a.Persister).Returns(persister);

            JournalViewModel journal = new JournalViewModel(new Journal(), app.Object);
            _localSource.Setup(s => s.SaveJournalAsync(It.IsAny<XDocument>(), journal)).Returns(Task.CompletedTask);
            persister.SaveAsync(journal).Wait();
        }

        [TestMethod]
        public void SaveAsyncThrowsTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;
            app.Setup(a => a.Persister).Returns(persister);

            JournalViewModel journal = new JournalViewModel(new Journal(), app.Object);
            _localSource.Setup(s => s.SaveJournalAsync(It.IsAny<XDocument>(), journal)).Throws(new Exception());
            try
            {
                persister.SaveAsync(journal).Wait();
                Assert.Fail("Should've thrown");
            }
            catch (Exception)
            {
            }

            // Make sure that we don't deadlock on a second try.
            _localSource.Setup(s => s.SaveJournalAsync(It.IsAny<XDocument>(), journal)).Returns(Task.CompletedTask);
            persister.SaveAsync(journal).Wait();
        }

        [TestMethod]
        [Ignore]
        public void SaveAsyncRaceConditionSuccess()
        {
            _quitFirstTask = false;

            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;
            app.Setup(a => a.Persister).Returns(persister);

            JournalViewModel journal = new JournalViewModel(new Journal(), app.Object);
            _localSource.Setup(s => s.SaveJournalAsync(It.IsAny<XDocument>(), journal)).Returns(FirstTaskVoid());
            persister.SaveAsync(journal).Wait();

            // Make sure that the second call isn't called until the first one is finished.
            _localSource.Setup(s => s.SaveJournalAsync(It.IsAny<XDocument>(), journal)).Returns(Task.CompletedTask);
            Task t = persister.SaveAsync(journal);
            Thread.Sleep(250);
            Assert.IsFalse(t.IsCompleted);
            _quitFirstTask = true;
            Thread.Sleep(500);
            Assert.IsTrue(t.IsCompleted);
        }

        [TestMethod]
        public void SaveFileAsyncSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            MemoryStream stream = new MemoryStream();
            String identifier = "Identifier";
            _localSource.Setup(s => s.SaveFileAsync(stream, identifier)).Returns(Task.CompletedTask);
            persister.SaveFileAsync(stream, identifier, false).Wait();
        }

        [TestMethod]
        public void SaveFileAsyncGoToNextAvailableIdentifierTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            MemoryStream stream = new MemoryStream();
            String identifier = "Identifier";
            String expectedIdentifier = "Identifier3";
            _localSource.Setup(s => s.GetUniqueFileNameAsync(identifier)).Returns(ReturnObject(expectedIdentifier));
            _localSource.Setup(s => s.SaveFileAsync(stream, identifier)).Returns(Task.CompletedTask);
            persister.SaveFileAsync(stream, identifier, true).Wait();
        }

        [TestMethod]
        public void SaveFileAsyncThrowsTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            MemoryStream stream = new MemoryStream();
            String identifier = "Identifier";
            _localSource.Setup(s => s.SaveFileAsync(stream, identifier)).Throws(new Exception());
            try
            {
                persister.SaveFileAsync(stream, identifier, false).Wait();
                Assert.Fail("Should've thrown");
            }
            catch (Exception)
            {
            }

            // Make sure that we don't deadlock on a second try.
            _localSource.Setup(s => s.SaveFileAsync(stream, identifier)).Returns(Task.CompletedTask);
            persister.SaveFileAsync(stream, identifier, false).Wait();
        }

        [TestMethod]
        [Ignore]
        public void SaveFileAsyncRaceConditionSuccess()
        {
            _quitFirstTask = false;

            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            MemoryStream stream = new MemoryStream();
            String identifier = "Identifier";
            _localSource.Setup(s => s.SaveFileAsync(stream, identifier)).Returns(FirstTaskVoid());
            persister.SaveFileAsync(stream, identifier, true).Wait();

            // Make sure that the second call isn't called until the first one is finished.
            _localSource.Setup(s => s.SaveFileAsync(stream, identifier)).Returns(Task.CompletedTask);
            Task t = new Task(() =>
            {
                persister.SaveFileAsync(stream, identifier, true).Wait();
            });
            t.Start();
            Thread.Sleep(250);
            Assert.IsFalse(t.IsCompleted);
            _quitFirstTask = true;
            Thread.Sleep(500);
            Assert.IsTrue(t.IsCompleted);
        }

        [TestMethod]
        public void LoadAsyncSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);

            XDocument doc = new XDocument();
            _localSource.Setup(s => s.LoadJournalAsync()).Returns(ReturnObject(doc));
            persister.LoadAsync().Wait();
        }

        [TestMethod]
        public void LoadAsyncFromSourceSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);

            XDocument doc = new XDocument();
            _cloudSource.Setup(s => s.LoadJournalAsync()).Returns(ReturnObject(doc));
            persister.LoadFromSourceAsync(_cloudSource.Object).Wait();
        }

        [TestMethod]
        public void LoadAsyncThrowsTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);

            _localSource.Setup(s => s.LoadJournalAsync()).Throws(new Exception());
            try
            {
                persister.LoadAsync().Wait();
                Assert.Fail("Should've thrown");
            }
            catch (Exception)
            {
            }

            // Make sure that we don't deadlock on a second try.
            XDocument doc = new XDocument();
            _localSource.Setup(s => s.LoadJournalAsync()).Returns(ReturnObject(doc));
            persister.LoadAsync().Wait();
        }

        [TestMethod]
        [Ignore]
        public void LoadAsyncRaceConditionSuccess()
        {
            _quitFirstTask = false;

            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            XDocument doc = new XDocument();
            _localSource.Setup(s => s.LoadJournalAsync()).Returns(FirstTaskReturns(doc));
            persister.LoadAsync().Wait();

            // Make sure that the second call isn't called until the first one is finished.
            _localSource.Setup(s => s.LoadJournalAsync()).Returns(ReturnObject(doc));
            Task t = new Task(() =>
            {
                persister.LoadAsync().Wait();
            });
            t.Start();
            Thread.Sleep(250);
            Assert.IsFalse(t.IsCompleted);
            _quitFirstTask = true;
            Thread.Sleep(500);
            Assert.IsTrue(t.IsCompleted);
        }

        [TestMethod]
        public void LoadFileAsyncSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            String path = "Path";
            String identifier = "Identifier";
            _localSource.Setup(s => s.LoadFileAsync(identifier)).Returns(ReturnObject(path));
            persister.LoadFileAsync(identifier).Wait();
        }

        [TestMethod]
        public void LoadFileAsyncThrowsTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            String identifier = "Identifier";
            _localSource.Setup(s => s.LoadFileAsync(identifier)).Throws(new Exception());
            try
            {
                persister.LoadFileAsync(identifier).Wait();
                Assert.Fail("Should've thrown");
            }
            catch (Exception)
            {
            }

            // Make sure that we don't deadlock on a second try.
            String path = "Path";
            _localSource.Setup(s => s.LoadFileAsync(identifier)).Returns(ReturnObject(path));
            persister.LoadFileAsync(identifier).Wait();
        }

        [TestMethod]
        [Ignore]
        public void LoadFileAsyncRaceConditionSuccess()
        {
            _quitFirstTask = false;

            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            String path = "Path";
            String identifier = "Identifier";
            _localSource.Setup(s => s.LoadFileAsync(identifier)).Returns(FirstTaskReturns(path));
            persister.LoadFileAsync(identifier).Wait();

            // Make sure that the second call isn't called until the first one is finished.
            _localSource.Setup(s => s.LoadFileAsync(identifier)).Returns(ReturnObject(path));
            Task t = new Task(() =>
            {
                persister.LoadFileAsync(identifier).Wait();
            });
            t.Start();
            Thread.Sleep(250);
            Assert.IsFalse(t.IsCompleted);
            _quitFirstTask = true;
            Thread.Sleep(500);
            Assert.IsTrue(t.IsCompleted);
        }

        [TestMethod]
        public void ClearCacheAsyncSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            _localSource.Setup(s => s.ClearCacheAsync()).Returns(Task.CompletedTask);
            persister.ClearCacheAsync().Wait();
        }

        [TestMethod]
        public void ClearCacheAsyncThrowsTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            _localSource.Setup(s => s.ClearCacheAsync()).Throws(new Exception());
            try
            {
                persister.ClearCacheAsync().Wait();
                Assert.Fail("Should've thrown");
            }
            catch (Exception)
            {
            }

            // Make sure that we don't deadlock on a second try.
            String path = "Path";
            _localSource.Setup(s => s.ClearCacheAsync()).Returns(ReturnObject(path));
            persister.ClearCacheAsync().Wait();
        }

        [TestMethod]
        [Ignore]
        public void ClearCacheAsyncRaceConditionSuccess()
        {
            _quitFirstTask = false;

            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            _localSource.Setup(s => s.ClearCacheAsync()).Returns(FirstTaskVoid());
            persister.ClearCacheAsync().Wait();

            // Make sure that the second call isn't called until the first one is finished.
            _localSource.Setup(s => s.ClearCacheAsync()).Returns(Task.CompletedTask);
            Task t = new Task(() =>
            {
                persister.ClearCacheAsync().Wait();
            });
            t.Start();
            Thread.Sleep(250);
            Assert.IsFalse(t.IsCompleted);
            _quitFirstTask = true;
            Thread.Sleep(500);
            Assert.IsTrue(t.IsCompleted);
        }

        [TestMethod]
        public void DeleteFileAsyncSuccessTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            String identifier = "Identifier";
            _localSource.Setup(s => s.DeleteFileAsync(identifier)).Returns(Task.CompletedTask);
            persister.LoadFileAsync(identifier).Wait();
        }

        [TestMethod]
        public void DeleteFileAsyncThrowsTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            String identifier = "Identifier";
            _localSource.Setup(s => s.DeleteFileAsync(identifier)).Throws(new Exception());
            try
            {
                persister.DeleteFileAsync(identifier).Wait();
                Assert.Fail("Should've thrown");
            }
            catch (Exception)
            {
            }

            // Make sure that we don't deadlock on a second try.
            _localSource.Setup(s => s.DeleteFileAsync(identifier)).Returns(Task.CompletedTask);
            persister.LoadFileAsync(identifier).Wait();
        }

        [TestMethod]
        [Ignore]
        public void DeleteFileAsyncRaceConditionSuccess()
        {
            _quitFirstTask = false;

            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            MockAppPersister persister = new MockAppPersister(app.Object);
            SetupPersister(persister);
            persister.Source = _localSource.Object;

            String identifier = "Identifier";
            _localSource.Setup(s => s.DeleteFileAsync(identifier)).Returns(FirstTaskVoid());
            persister.DeleteFileAsync(identifier).Wait();

            // Make sure that the second call isn't called until the first one is finished.
            _localSource.Setup(s => s.DeleteFileAsync(identifier)).Returns(Task.CompletedTask);
            Task t = new Task(() =>
            {
                persister.DeleteFileAsync(identifier).Wait();
            });
            t.Start();
            Thread.Sleep(250);
            Assert.IsFalse(t.IsCompleted);
            _quitFirstTask = true;
            Thread.Sleep(500);
            Assert.IsTrue(t.IsCompleted);
        }

        #region Utility Functions
        bool _quitFirstTask = false;
        private async Task FirstTaskVoid()
        {
            while (!_quitFirstTask)
            {
                await Task.Delay(10);
            }
        }

        private async Task<T> FirstTaskReturns<T>(T obj)
        {
            await FirstTaskVoid();
            return obj;
        }

        private async Task<T> ReturnObject<T>(T obj)
        {
            await Task.CompletedTask;
            return obj;
        }

        private void SetupPersister(MockAppPersister persister)
        {
            if (_localSource == null)
            {
                _localSource = new Mock<IPersistenceSource>();
                persister.AddTestSource(_localSource.Object);
            }
            if (_cloudSource == null)
            {
                _cloudSource = new Mock<IPersistenceSource>();
                persister.AddTestSource(_cloudSource.Object);
            }
        }
        #endregion
    }
}
