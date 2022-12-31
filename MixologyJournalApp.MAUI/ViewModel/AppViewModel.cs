using MixologyJournalApp.MAUI.Data;
using MixologyJournalApp.MAUI.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.MAUI.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private LocalDatabase _database = null;

        public ObservableCollection<UnitViewModel> Units { get; set; } = new();

        private bool _hasBeenInitialized = false;

        public AppViewModel()
        {
            this._database = new LocalDatabase();
        }

        internal async Task InitializeAsync() 
        {
            if (!this._hasBeenInitialized)
            {
                List<Unit> items = await this._database.GetItemsAsync<Unit>();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (Unit item in items)
                    {
                        this.Units.Add(new UnitViewModel(item));
                    }
                });
                this._hasBeenInitialized = true;
            }
        }
    }
}
