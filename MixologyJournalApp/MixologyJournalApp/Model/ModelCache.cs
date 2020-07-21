﻿using MixologyJournalApp.Platform;
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

        public async Task Init()
        {
            if (DateTime.UtcNow.Subtract(LastLoadedTime).TotalMilliseconds > TimeSpan.FromHours(1).TotalMilliseconds)
            {
                try
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
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                    _canAccessRemote = false;
                }
            }
            InitProgress = 4.0;
        }

        public async Task<Boolean> UploadRecentItems()
        {
            Boolean result = true;
            try
            {
                foreach (Recipe recipe in Recipes.Where(r => !r.Uploaded))
                {
                    result = result && await UploadRecipe(recipe);
                }

                foreach (Drink drink in Drinks.Where(d => !d.Uploaded))
                {
                    result = result && await UploadDrink(drink);
                }
            }
            catch (HttpRequestException)
            {
                result = false;
            }
            return result;
        }

        private async Task UpdateRecipes()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/recipes");
                    List<Recipe> recipeModels = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult);

                    if (GetUseRemote())
                    {
                        _recipes.Clear();
                    }

                    foreach (Recipe r in recipeModels.OrderBy(i => i.Name))
                    {
                        r.Uploaded = true;
                        _recipes.Add(r);
                    }
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
        }

        private async Task UpdateDrinks()
        {
            // If we are local only, then we won't get any drinks from the remote backend.
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/drinks");
                    List<Drink> drinkModels = JsonConvert.DeserializeObject<List<Drink>>(jsonResult);
                    drinkModels.ForEach(d => d.Init(Recipes.FirstOrDefault(r => r.Id.Equals(d.SourceRecipeID))));

                    if (GetUseRemote())
                    {
                        _drinks.Clear();
                    }

                    foreach (Drink d in drinkModels.OrderBy(i => i.Name))
                    {
                        d.Uploaded = true;
                        _drinks.Add(d);
                    }
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
        }

        private async Task UpdateAvailableIngredients()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/ingredients");
                    List<Ingredient> ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonResult).ToList();

                    if (GetUseRemote())
                    {
                        _ingredients.Clear();
                    }

                    foreach (Ingredient i in ingredients.OrderBy(i => i.Name))
                    {
                        _ingredients.Add(i);
                    }
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
        }

        private async Task UpdateAvailableUnits()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/units");
                    List<Unit> units = JsonConvert.DeserializeObject<List<Unit>>(jsonResult).ToList();

                    if (GetUseRemote())
                    {
                        _units.Clear();
                    }

                    foreach (Unit u in units.OrderBy(i => i.Name))
                    {
                        _units.Add(u);
                    }
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
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
                finalResult = await UploadRecipe(model);
            }
            else
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

        private async Task<bool> UploadRecipe(Recipe model)
        {
            if (_canAccessRemote)
            {
                QueryResult result = await _app.PlatformInfo.Backend.PostResult("/secure/recipes", model);
                if (result.Result)
                {
                    model.Id = result.Content["createdId"];
                    model.Uploaded = true;
                }
                else
                {
                    WarnAboutRemoteAccessibility();
                }
                return result.Result;
            }
            return false;
        }

        internal async Task<Boolean> DeleteRecipe(Recipe recipe)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                QueryResult result = await _app.PlatformInfo.Backend.DeleteResult("/secure/recipes", recipe);
                finalResult = result.Result;
            }

            if (!finalResult)
            {
                WarnAboutRemoteAccessibility();
            }

            _recipes.Remove(recipe);

            Save();

            return finalResult;
        }

        internal async Task<Boolean> CreateDrink(Drink model)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                finalResult = await UploadDrink(model);
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

        private async Task<bool> UploadDrink(Drink model)
        {
            if (_canAccessRemote)
            {
                QueryResult result = await _app.PlatformInfo.Backend.PostResult("/secure/drinks", model);
                if (result.Result)
                {
                    model.Uploaded = true;
                    model.Id = result.Content["createdId"];
                }
                else
                {
                    WarnAboutRemoteAccessibility();
                }

                return result.Result;
            }
            return false;
        }

        internal async Task<Boolean> DeleteDrink(Drink drink)
        {
            Boolean finalResult = true;
            if (GetUseRemote())
            {
                QueryResult result = await _app.PlatformInfo.Backend.DeleteResult("/secure/drinks", drink);
                finalResult = result.Result;

                if (!finalResult)
                {
                    WarnAboutRemoteAccessibility();
                }
            }

            _drinks.Remove(drink);

            Save();

            return finalResult;
        }

        internal async Task UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            if (GetUseRemote())
            {
                QueryResult result = await _app.PlatformInfo.Backend.PostResult("/secure/favorite", new FavoriteRequest(drink.SourceRecipeID, drink.Id, isFavorite));
                if (!result.Result)
                {
                    WarnAboutRemoteAccessibility();
                }
            }

            Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void WarnAboutRemoteAccessibility()
        {
            if (_canAccessRemote)
            {
                _app.PlatformInfo.AlertDialogFactory.ShowDialog("Server Down",
                                        "The backend server appears to be down. We will use the local cache, but are unable to retrieve any updated content from the servers at this time.\n" +
                                        "Additionally, any changes made will be kept locally until we are able to sync with the backend again.");
            }
            _canAccessRemote = false;
        }
    }
}
