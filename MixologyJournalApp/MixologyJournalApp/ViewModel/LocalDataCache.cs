using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class LocalDataCache: INotifyPropertyChanged
    {
        private readonly App _app;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RecipeViewModel> Recipes { get; } = new ObservableCollection<RecipeViewModel>();

        public ObservableCollection<DrinkViewModel> Drinks { get; } = new ObservableCollection<DrinkViewModel>();

        public ObservableCollection<IngredientViewModel> AvailableIngredients { get; } = new ObservableCollection<IngredientViewModel>();

        public ObservableCollection<UnitViewModel> AvailableUnits { get; } = new ObservableCollection<UnitViewModel>();

        private double _initProgress = 0.0;
        public double InitProgress
        {
            get
            {
                return _initProgress;
            }
            private set
            {
                _initProgress = value;
                OnPropertyChanged(nameof(InitProgress));
            }
        }

        public LocalDataCache(App app)
        {
            _app = app;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Init()
        {
            await UpdateAvailableUnits();
            InitProgress = 0.25;
            await UpdateAvailableIngredients();
            InitProgress = 0.5;
            List<Recipe> recipeModels = await UpdateRecipes();
            InitProgress = 0.75;
            await UpdateDrinks(recipeModels);
            InitProgress = 1.0;
        }

        public async Task Resync()
        {
            await Init();
        }

        private async Task<List<Recipe>> UpdateRecipes()
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/recipes");
            List<Recipe> recipeModels = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult);
            List<RecipeViewModel> recipes = recipeModels.Select(r => new RecipeViewModel(r, _app)).ToList();
            Recipes.Clear();
            foreach (RecipeViewModel r in recipes.OrderBy(i => i.Name))
            {
                Recipes.Add(r);
            }
            return recipeModels;
        }

        private async Task UpdateDrinks(List<Recipe> recipeModels)
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/drinks");
            List<Drink> drinkModels = JsonConvert.DeserializeObject<List<Drink>>(jsonResult);
            drinkModels.ForEach(d => d.Init(recipeModels.FirstOrDefault(r => r.Id.Equals(d.SourceRecipeID))));
            List<DrinkViewModel> drinks = drinkModels.Select(d => new DrinkViewModel(d, _app)).ToList();
            Drinks.Clear();
            foreach (DrinkViewModel d in drinks.OrderBy(i => i.Name))
            {
                Drinks.Add(d);
            }
        }

        private async Task UpdateAvailableIngredients()
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/ingredients");
            List<IngredientViewModel> ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonResult).Select(i => new IngredientViewModel(i)).ToList();
            AvailableIngredients.Clear();
            foreach (IngredientViewModel i in ingredients.OrderBy(i => i.Name))
            {
                AvailableIngredients.Add(i);
            }
        }

        private async Task UpdateAvailableUnits()
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/units");
            List<UnitViewModel> units = JsonConvert.DeserializeObject<List<Unit>>(jsonResult).Select(u => new UnitViewModel(u)).ToList();
            AvailableUnits.Clear();
            foreach (UnitViewModel u in units.OrderBy(i => i.Name))
            {
                AvailableUnits.Add(u);
            }
        }

        public async Task<Boolean> CreateRecipe(RecipeViewModel viewModel, Recipe model)
        {
            QueryResult result = await _app.PlatformInfo.Backend.PostResult("/secure/recipes", model);

            if (result.Result)
            {
                model.Id = result.Content["createdId"];

                RecipeViewModel insertBeforeRecipe = Recipes.FirstOrDefault(r => viewModel.Name.CompareTo(r.Name) < 0);
                if (insertBeforeRecipe == null)
                {
                    Recipes.Add(viewModel);
                }
                else
                {
                    int insertIndex = Recipes.IndexOf(insertBeforeRecipe);
                    Recipes.Insert(insertIndex, viewModel);
                }
            }
            return result.Result;
        }

        public async Task DeleteRecipe(Recipe recipe)
        {
            QueryResult result = await _app.PlatformInfo.Backend.DeleteResult("/secure/recipes", recipe);
            if (result.Result)
            {
                RecipeViewModel recipeViewModel = Recipes.FirstOrDefault(d => d.Id == recipe.Id);
                Recipes.Remove(recipeViewModel);
            }
        }

        public async Task<Boolean> CreateDrink(DrinkViewModel viewModel, Drink model)
        {
            QueryResult result = await _app.PlatformInfo.Backend.PostResult("/secure/drinks", model);

            if (result.Result)
            {
                model.Id = result.Content["createdId"];

                DrinkViewModel insertBeforeRecipe = Drinks.FirstOrDefault(d => viewModel.Name.CompareTo(d.Name) < 0);
                if (insertBeforeRecipe == null)
                {
                    Drinks.Add(viewModel);
                }
                else
                {
                    int insertIndex = Drinks.IndexOf(insertBeforeRecipe);
                    Drinks.Insert(insertIndex, viewModel);
                }
            }
            return result.Result;
        }

        public async Task DeleteDrink(Drink drink)
        {
            QueryResult result = await _app.PlatformInfo.Backend.DeleteResult("/secure/drinks", drink);
            if (result.Result)
            {
                DrinkViewModel drinkViewModel = Drinks.FirstOrDefault(d => d.Id == drink.Id);
                Drinks.Remove(drinkViewModel);
            }
        }

        public async Task UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            await _app.PlatformInfo.Backend.PostResult("/secure/favorite", new FavoriteRequest(drink.SourceRecipeID, drink.Id, isFavorite));
        }
    }
}
