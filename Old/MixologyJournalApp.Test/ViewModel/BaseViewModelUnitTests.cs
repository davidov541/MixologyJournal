using System;
using System.IO;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using Moq;

namespace MixologyJournalApp.Test.ViewModel
{
    public abstract class BaseViewModelUnitTests
    {
        protected BaseMixologyApp CreateTestApp()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            // We return specific values only for the ones we know we care about in tests, and then the others are just a bogus value.
            // This isn't great, but it gets us passing tests without relying on a platform's implementation of localization.
            app.Setup(a => a.GetLocalizedString(It.IsAny<String>())).Returns<String>((s) =>
            {
                switch (s)
                {
                    case "OunceUnitPlural":
                        return "Ounces";
                    case "CentiliterUnitPlural":
                        return "Centiliters";
                    case "OunceUnitFormat":
                    case "CentiliterUnitFormat":
                        return "{0} {1} of {2}";
                    default:
                        return "Localized";
                }
            });
            Mock<BaseAppPersister> persister = new Mock<BaseAppPersister>(app.Object);
            app.Setup(a => a.Persister).Returns(persister.Object);
            Mock<IPersistenceSource> persistenceSource = new Mock<IPersistenceSource>();
            persister.Setup(p => p.Source).Returns(persistenceSource.Object);
            persistenceSource.Setup(p => p.GetUniqueFileNameAsync(It.IsAny<String>())).Returns(ReturnUniqueFileName());
            JournalViewModel journal = new JournalViewModel(new Journal(), app.Object);
            app.Setup(a => a.Journal).Returns(journal);
            return app.Object;
        }

        private async Task<String> ReturnUniqueFileName()
        {
            await Task.Yield();
            return "Picture";
        }

        protected void StartTimingTest()
        {
            _continue = false;
        }

        protected void FinishTimingTest()
        {
            _continue = true;
        }

        private bool _continue;
        protected async Task<Stream> GetPictureTaskTest()
        {
            while (!_continue)
            {
                await Task.Delay(10);
            }
            return new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        }

        protected async Task WaitForSaveTest()
        {
            while (!_continue)
            {
                await Task.Delay(10);
            }
        }
    }
}
