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

        public ObservableCollection<UnitViewModel> Units 
        { 
            get;
            private set;
        } = new();

        public ObservableCollection<RecipeViewModel> Recipes
        {
            get;
            private set;
        } = new();

        public bool Initializing
        {
            get;
            private set;
        }

        private bool _hasBeenInitialized = false;

        public AppViewModel()
        {
            this._database = new LocalDatabase();
        }

        internal async Task InitializeAsync() 
        {
            if (!this._hasBeenInitialized && !this.Initializing)
            {
                this.Initializing = true;
                List<Unit> items = await this._database.GetItemsAsync<Unit>();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (Unit item in items)
                    {
                        this.Units.Add(new UnitViewModel(item));
                    }
                });
                List<Recipe> recipes = await this._database.GetItemsAsync<Recipe>();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (Recipe item in recipes)
                    {
                        this.Recipes.Add(new RecipeViewModel(item));
                    }
                });
                this._hasBeenInitialized = true;
                this.Initializing = false;
            }
        }
    }
}
