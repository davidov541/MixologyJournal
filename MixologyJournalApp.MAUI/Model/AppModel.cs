using MixologyJournalApp.MAUI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.MAUI.Model
{
    internal class AppModel : INotifyPropertyChanged
    {
        private LocalDatabase _database = null;
        private bool _hasBeenInitialized = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _initializing = false;
        public bool Initializing
        {
            get
            {
                return this._initializing;
            }
            private set
            {
                this._initializing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Initializing)));
            }
        }

        public ObservableCollection<Unit> Units
        {
            get;
            private set;
        } = new();

        public ObservableCollection<Recipe> Recipes
        {
            get;
            private set;
        } = new();

        internal AppModel()
        {
            this._database = new LocalDatabase();
        }

        internal async Task InitializeAsync()
        {
            if (!this._hasBeenInitialized && !this.Initializing)
            {
                this.Initializing = true;
                List<Unit> items = await this._database.LoadAllModels<Unit>();
                foreach (Unit item in items)
                {
                    this.Units.Add(item);
                }
                List<Recipe> recipes = await this._database.LoadAllModels<Recipe>();
                foreach (Recipe item in recipes)
                {
                    this.Recipes.Add(item);
                }
                this._hasBeenInitialized = true;
                this.Initializing = false;
            }
        }
    }
}
