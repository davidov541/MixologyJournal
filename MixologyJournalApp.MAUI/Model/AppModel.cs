using MixologyJournalApp.MAUI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.MAUI.Model
{
    internal class AppModel : INotifyPropertyChanged, IAppCache
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

        public ObservableCollection<Ingredient> Ingredients
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

                List<Unit> units = await this._database.LoadAllModels<Unit>();
                foreach (Unit unit in units)
                {
                    this.Units.Add(unit);
                }

                List<Ingredient> ingredients = await this._database.LoadAllModels<Ingredient>();
                foreach (Ingredient ingredient in ingredients)
                {
                    this.Ingredients.Add(ingredient);
                }
 
                List<Recipe> recipes = await this._database.LoadAllModels<Recipe>();
                foreach (Recipe recipe in recipes)
                {
                    await recipe.LoadAsync(this, this._database);
                    this.Recipes.Add(recipe);
                }

                this._hasBeenInitialized = true;
                this.Initializing = false;
            }
        }

        public Recipe GetRecipeById(string id)
        {
            Recipe match = this.Recipes.SingleOrDefault(r => r.Id == id);
            if (match == null)
            {
                throw new KeyNotFoundException($"ID {id} was not found in the list of recipes for this app.");
            }
            return match;
        }

        public Unit GetUnitById(string id)
        {
            Unit match = this.Units.SingleOrDefault(r => r.Id == id);
            if (match == null)
            {
                throw new KeyNotFoundException($"ID {id} was not found in the list of units for this app.");
            }
            return match;
        }

        public Ingredient GetIngredientById(string id)
        {
            Ingredient match = this.Ingredients.SingleOrDefault(r => r.Id == id);
            if (match == null)
            {
                throw new KeyNotFoundException($"ID {id} was not found in the list of ingredients for this app.");
            }
            return match;
        }
    }
}
