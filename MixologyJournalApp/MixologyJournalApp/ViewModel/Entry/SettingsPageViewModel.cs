using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Entry
{
    public interface ISettingsPageViewModel : IPageViewModel
    {
        Task ClearCacheAsync();
    }

    internal class SettingsPageViewModel : INotifyPropertyChanged, ISettingsPageViewModel
    {
        private IPersistenceSource _syncSource;
        private BaseMixologyApp _app;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ShouldSync
        {
            get
            {
                return !_app.Persister.Source.IsLocal;
            }
            set
            {
                IPersistenceSource newSource = _app.Persister.Sources.SingleOrDefault(s => s.IsLocal);
                if (value)
                {
                    newSource = _syncSource;
                }
                SetSourceAsync(newSource).ContinueWith(t =>
                {
                    OnPropertyChanged(nameof(ShouldSync));
                    OnPropertyChanged(nameof(Source));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public String Source
        {
            get
            {
                return _syncSource.Name;
            }
            set
            {
                SetSourceAsync(_app.Persister.Sources.SingleOrDefault(s => s.Name.Equals(value))).ContinueWith(t =>
                {
                    _syncSource = _app.Persister.Source;
                    OnPropertyChanged(nameof(Source));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public IEnumerable<String> Sources
        {
            get
            {
                return _app.Persister.Sources.Where(s => !s.IsLocal).Select(s => s.Name);
            }
        }

        private bool _busy;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            private set
            {
                _busy = value;
                OnPropertyChanged(nameof(Busy));
            }
        }

        public SettingsPageViewModel(BaseMixologyApp app)
        {
            _app = app;
            // Set sync source to the first non-local source so that something is chosen.
            _syncSource = _app.Persister.Sources.FirstOrDefault(s => !s.IsLocal);
        }

        protected void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SetSourceAsync(IPersistenceSource source)
        {
            Busy = true;
            bool useCurrent;
            IPersistenceSource oldSource = _app.Persister.Source;
            JournalViewModel newJournal = await _app.Persister.LoadFromSourceAsync(source) as JournalViewModel;
            if (newJournal == null)
            {
                // Something bad happened, so move on.
                await _app.DisplayDialogAsync(
                    _app.GetLocalizedString("ErrorDialogHeader"),
                    _app.GetLocalizedString("CouldNotConnectDialogDescription"),
                    _app.GetLocalizedString("OKOption"));

                Busy = false;
                return;
            }
            if (!newJournal.IsEmpty && !_app.Journal.IsEmpty)
            {
                String newSourceName = _app.Persister.Source.Name;
                String message = String.Format(_app.GetLocalizedString("ConflictDialogDescriptionFormatted"), newSourceName);
                useCurrent = await _app.DisplayDialogAsync(
                    _app.GetLocalizedString("ConflictDialogHeader"),
                    message,
                    _app.GetLocalizedString("KeepOption"),
                    String.Format(_app.GetLocalizedString("UseOptionFormatted"), newSourceName));
            }
            else
            {
                useCurrent = !_app.Journal.IsEmpty;
            }
            _app.Persister.Source = source;
            await _app.UpdateJournalAsync(useCurrent, oldSource);
            Busy = false;
        }

        public async Task ClearCacheAsync()
        {
            Busy = true;
            await (_app.Persister as BaseAppPersister).ClearCacheAsync();
            Busy = false;
        }
    }
}
