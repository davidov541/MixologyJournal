using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ModelCache : IDisposable, INotifyPropertyChanged
    {
        internal const int InitStepsCount = 4;

        private List<Recipe> _recipes = new List<Recipe>();
        [JsonProperty("recipes")]
        public IEnumerable<Recipe> Recipes
        {
            get
            {
                return _recipes;
            }
            set
            {
                _recipes = new List<Recipe>(value);
            }
        }

        private List<Drink> _drinks = new List<Drink>();
        [JsonProperty("drinks")]
        public IEnumerable<Drink> Drinks
        {
            get
            {
                return _drinks;
            }
            set
            {
                _drinks = new List<Drink>(value);
            }
        }

        private List<Ingredient> _ingredients = new List<Ingredient>();
        [JsonProperty("ingredients")]
        public IEnumerable<Ingredient> AvailableIngredients
        {
            get
            {
                return _ingredients;
            }
            set
            {
                _ingredients = new List<Ingredient>(value);
            }
        }

        private List<Unit> _units = new List<Unit>();
        [JsonProperty("units")]
        public IEnumerable<Unit> AvailableUnits
        {
            get
            {
                return _units;
            }
            set
            {
                _units = new List<Unit>(value);
            }
        }

        [JsonProperty("lastLoadedTime")]
        public DateTime LastLoadedTime
        {
            get;
            set;
        }

        private Boolean _canAccessRemote = true;
        private Boolean GetUseRemote()
        {
            return _app.PlatformInfo.Authentication.IsAuthenticated && _canAccessRemote;
        }

        private double _initProgress = 0.0;
        public double InitProgress
        {
            get
            {
                return _initProgress / InitStepsCount;
            }
            private set
            {
                _initProgress = value;
                OnPropertyChanged(nameof(InitProgress));
            }
        }

        public static ModelCache Create(App app)
        {
            String fileName = GetSerializationPath();
            if (File.Exists(fileName))
            {
                String jsonString = File.ReadAllText(fileName);
                ModelCache cache = JsonConvert.DeserializeObject<ModelCache>(jsonString);
                cache._app = app;
                return cache;
            }
            return new ModelCache(app);
        }

        private App _app;

        public ModelCache()
        {
            LastLoadedTime = DateTime.MinValue;
        }

        private ModelCache(App app) : this()
        {
            _app = app;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Init()
        {
            if (DateTime.UtcNow.Subtract(LastLoadedTime).TotalMilliseconds > TimeSpan.FromHours(1).TotalMilliseconds)
            {
                InitProgress = 0.0;
                await UpdateAvailableUnits();

                InitProgress = 1.0;
                await UpdateAvailableIngredients();

                InitProgress = 2.0;
                await UpdateRecipes();

                InitProgress = 3.0;
                await UpdateDrinks();

                LastLoadedTime = DateTime.UtcNow;
            }
            InitProgress = 4.0;
        }

        internal async Task<Boolean> UploadRecentItems()
        {
            Boolean result = true;
            try
            {
                foreach (Recipe recipe in Recipes.Where(r => !r.Uploaded))
                {
                    result = result && await _app.PlatformInfo.Backend.UploadRecipe(recipe);
                }

                foreach (Drink drink in Drinks.Where(d => !d.Uploaded))
                {
                    result = result && await _app.PlatformInfo.Backend.UploadDrink(drink);
                }

                foreach (Drink drink in Drinks.Where(d => !d.IsFavoriteUploaded))
                {
                    result = result && await UpdateFavoriteDrink(drink, drink.IsFavorite);
                }
            }
            catch (HttpRequestException)
            {
                result = false;
            }
            return result;
        }

        private static String GetSerializationPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "modelcache.json");
        }

        internal void Save()
        {
            String serializationPath = GetSerializationPath();
            String serializedCache = JsonConvert.SerializeObject(this);
            File.WriteAllText(serializationPath, serializedCache);
        }

        public void Dispose()
        {
            Save();
        }

        internal async Task<Boolean> CreateRecipe(Recipe model)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                finalResult = await _app.PlatformInfo.Backend.UploadRecipe(model);
            }

            if (String.IsNullOrEmpty(model.Id))
            {
                // Make a random GUID for us to use in the meantime.
                model.Id = Guid.NewGuid().ToString();
            }

            Recipe insertBeforeRecipe = Recipes.FirstOrDefault(r => model.Name.CompareTo(r.Name) < 0);
            if (insertBeforeRecipe == null)
            {
                _recipes.Add(model);
            }

            if (String.IsNullOrEmpty(model.Id))
            {
                int insertIndex = _recipes.IndexOf(insertBeforeRecipe);
                _recipes.Insert(insertIndex, model);
            }

            Save();

            return finalResult;
        }

        internal async Task<Boolean> CreateDrink(Drink model)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                finalResult = await _app.PlatformInfo.Backend.UploadDrink(model);
            }

            if (String.IsNullOrEmpty(model.Id))
            {
                // Make a random GUID for us to use in the meantime.
                model.Id = Guid.NewGuid().ToString();
            }

            Drink insertBeforeRecipe = Drinks.FirstOrDefault(d => model.Name.CompareTo(d.Name) < 0);
            if (insertBeforeRecipe == null)
            {
                _drinks.Add(model);
            }
            else
            {
                int insertIndex = _drinks.IndexOf(insertBeforeRecipe);
                _drinks.Insert(insertIndex, model);
            }

            Save();

            return finalResult;
        }

        public async Task AddPicture(Recipe model, String path)
        {
            model.Picture = await _app.PlatformInfo.Backend.UploadPicture(path);
        }

        public async Task AddPicture(Drink model, String path)
        {
            model.Picture = await _app.PlatformInfo.Backend.UploadPicture(path);
        }

        #region BackendInterface
        private async Task UpdateRecipes()
        {
            List<Recipe> recipeModels = await _app.PlatformInfo.Backend.UpdateRecipes();

            if (GetUseRemote() && recipeModels.Any())
            {
                _recipes.Clear();
            }

            foreach (Recipe r in recipeModels.OrderBy(i => i.Name))
            {
                r.Uploaded = true;
                _recipes.Add(r);
            }
        }

        private async Task UpdateDrinks()
        {
            List<Drink> drinkModels = await _app.PlatformInfo.Backend.UpdateDrinks();
            drinkModels.ForEach(d => d.Init(Recipes.FirstOrDefault(r => r.Id.Equals(d.SourceRecipeID))));

            if (GetUseRemote() && drinkModels.Any())
            {
                _drinks.Clear();
            }

            foreach (Drink d in drinkModels.OrderBy(i => i.Name))
            {
                d.Uploaded = true;
                d.IsFavoriteUploaded = true;
                _drinks.Add(d);
            }
        }

        private async Task UpdateAvailableIngredients()
        {
            List<Ingredient> ingredients = await _app.PlatformInfo.Backend.UpdateAvailableIngredients();

            if (GetUseRemote() && ingredients.Any())
            {
                _ingredients.Clear();
            }

            foreach (Ingredient i in ingredients.OrderBy(i => i.Name))
            {
                _ingredients.Add(i);
            }
        }

        private async Task UpdateAvailableUnits()
        {
            List<Unit> units = await _app.PlatformInfo.Backend.UpdateAvailableUnits();

            if (GetUseRemote() && units.Any())
            {
                _units.Clear();
            }

            foreach (Unit u in units.OrderBy(i => i.Name))
            {
                _units.Add(u);
            }
        }

        internal async Task<Boolean> UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                finalResult = await _app.PlatformInfo.Backend.UpdateFavoriteDrink(drink, isFavorite);
            }

            Save();

            return finalResult;
        }

        internal async Task<Boolean> DeleteRecipe(Recipe recipe)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                finalResult = await _app.PlatformInfo.Backend.DeleteRecipe(recipe);
            }

            _recipes.Remove(recipe);

            Save();

            return finalResult;
        }

        internal async Task<Boolean> DeleteDrink(Drink drink)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                finalResult = await _app.PlatformInfo.Backend.DeleteDrink(drink);
            }

            _drinks.Remove(drink);

            Save();

            return finalResult;
        }
        #endregion
    }
}
