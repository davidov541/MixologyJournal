using MixologyJournalApp.Model;
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

        private readonly ModelCache _modelCache;

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
            _modelCache = ModelCache.Create(app.PlatformInfo.Backend);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Init()
        {
            InitProgress = 0.0;
            await _modelCache.Init();

            InitProgress = 0.2;
            UpdateAvailableUnits();

            InitProgress = 0.4;
            UpdateAvailableIngredients();

            InitProgress = 0.6;
            UpdateRecipes();

            InitProgress = 0.8;
            UpdateDrinks();

            InitProgress = 1.0;
        }

        public async Task Resync()
        {
            await Init();
        }

        private void UpdateRecipes()
        {
            List<RecipeViewModel> recipes = _modelCache.Recipes.Select(r => new RecipeViewModel(r, _app)).ToList();
            Recipes.Clear();
            foreach (RecipeViewModel r in recipes.OrderBy(i => i.Name))
            {
                Recipes.Add(r);
            }
        }

        private void UpdateDrinks()
        {
            List<DrinkViewModel> drinks = _modelCache.Drinks.Select(d => new DrinkViewModel(d, _app)).ToList();
            Drinks.Clear();
            foreach (DrinkViewModel d in drinks.OrderBy(i => i.Name))
            {
                Drinks.Add(d);
            }
        }

        private void UpdateAvailableIngredients()
        {
            List<IngredientViewModel> ingredients = _modelCache.AvailableIngredients.Select(i => new IngredientViewModel(i)).ToList();
            AvailableIngredients.Clear();
            foreach (IngredientViewModel i in ingredients.OrderBy(i => i.Name))
            {
                AvailableIngredients.Add(i);
            }
        }

        private void UpdateAvailableUnits()
        {
            List<UnitViewModel> units = _modelCache.AvailableUnits.Select(u => new UnitViewModel(u)).ToList();
            AvailableUnits.Clear();
            foreach (UnitViewModel u in units.OrderBy(i => i.Name))
            {
                AvailableUnits.Add(u);
            }
        }

        public async Task<Boolean> CreateRecipe(RecipeViewModel viewModel, Recipe model)
        {
            Boolean result = await _modelCache.CreateRecipe(model);

            if (result)
            {
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
            return result;
        }

        public async Task DeleteRecipe(Recipe recipe)
        {
            Boolean result = await _modelCache.DeleteRecipe(recipe);
            if (result)
            {
                RecipeViewModel recipeViewModel = Recipes.FirstOrDefault(d => d.Id == recipe.Id);
                Recipes.Remove(recipeViewModel);
            }
        }

        public async Task<Boolean> CreateDrink(DrinkViewModel viewModel, Drink model)
        {
            Boolean result = await _modelCache.CreateDrink(model);

            if (result)
            {
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
            return result;
        }

        public async Task DeleteDrink(Drink drink)
        {
            Boolean result = await _modelCache.DeleteDrink(drink);
            if (result)
            {
                DrinkViewModel drinkViewModel = Drinks.FirstOrDefault(d => d.Id == drink.Id);
                Drinks.Remove(drinkViewModel);
            }
        }

        public async Task UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            await _modelCache.UpdateFavoriteDrink(drink, isFavorite);
        }
    }
}
