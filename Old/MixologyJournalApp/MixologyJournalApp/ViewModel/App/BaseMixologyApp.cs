using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.Entry;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournal.ViewModel.App
{
    public abstract class BaseMixologyApp
    {
        #region Public API
        public async Task InitializeAsync()
        {
            _journal = await Persister.LoadAsync() as JournalViewModel;
            while (Journal == null)
            {
                if (await DisplayDialogAsync(
                    GetLocalizedString("ErrorDialogHeader"),
                    GetLocalizedString("CouldNotConnectDescription"),
                    GetLocalizedString("ContinueDialogOption"),
                    GetLocalizedString("RetryDialogOption")
                ))
                {
                    Persister.Source = Persister.Sources.First(s => s.IsLocal);
                    await UpdateJournalAsync(false, null /* Doesn't Matter */);
                }
                else
                {
                    _journal = await Persister.LoadAsync() as JournalViewModel;
                }
            }
        }

        public IOverviewPageViewModel GetOverviewPage()
        {
            return new OverviewPageViewModel(this);
        }

        public IBaseRecipePageViewModel GetBaseRecipePage(int id)
        {
            return _journal.GetRecipeByID(id);
        }

        public IBaseRecipeEditPageViewModel GetBaseRecipeEditPage(int id)
        {
            return _journal.GetRecipeByID(id);
        }
        #endregion

        #region Extension Points
        protected internal abstract BaseAppPersister Persister
        {
            get;
        }

        protected BaseMixologyApp()
        {
            _places = new GooglePlacesProvider();
        }

        internal protected abstract Task<Stream> GetNewPictureAsync();

        internal protected abstract Task<Stream> GetExistingPictureAsync(UInt64 uploadLimit);

        internal protected abstract Task DisplayDialogAsync(String title, String content, String okText);

        internal protected abstract Task<bool> DisplayDialogAsync(String title, String content, String yesText, String noText);

        internal protected abstract String GetLocalizedString(String key);
        #endregion

        #region Internal API
        private JournalViewModel _journal;
        internal JournalViewModel Journal
        {
            get
            {
                return _journal;
            }
        }

        internal async Task UpdateJournalAsync(bool keepCurrent, IPersistenceSource oldSource)
        {
            if (keepCurrent)
            {
                await Persister.SaveAsync(Journal);
                await Persister.PortJournalToNewSourceAsync(oldSource);
            }
            else
            {
                _journal = await Persister.LoadAsync() as JournalViewModel;
            }
            await Persister.ClearCacheAsync();
        }
        #endregion

        #region Private Methods
        private GooglePlacesProvider _places;
        // TODO: This needs to actually be used once we get around to implementing the bar feature.
        public INearbyPlacesProvider NearbyPlaces
        {
            get
            {
                return _places;
            }
        }
        #endregion
    }
}
