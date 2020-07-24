using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class LocalDataCache: INotifyPropertyChanged, IDisposable
    {
        private const int InitStepsCount = 5 + ModelCache.InitStepsCount;
        private readonly App _app;

        private readonly ModelCache _modelCache;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RecipeViewModel> Recipes { get; } = new ObservableCollection<RecipeViewModel>();

        public ObservableCollection<DrinkViewModel> Drinks { get; } = new ObservableCollection<DrinkViewModel>();

        public ObservableCollection<IngredientViewModel> AvailableIngredients { get; } = new ObservableCollection<IngredientViewModel>();

        public ObservableCollection<UnitViewModel> AvailableUnits { get; } = new ObservableCollection<UnitViewModel>();

        private double _initStepsCompleted = 0.0;
        public double InitProgress
        {
            get
            {
                double modelCacheSteps = _modelCache.InitProgress * ModelCache.InitStepsCount;
                return (_initStepsCompleted + modelCacheSteps) / InitStepsCount;
            }
            private set
            {
                _initStepsCompleted = value;
                OnPropertyChanged(nameof(InitProgress));
            }
        }

        public LocalDataCache(App app)
        {
            _app = app;
            _modelCache = ModelCache.Create(app);
            _modelCache.PropertyChanged += ModelCache_PropertyChanged;
        }

        private void ModelCache_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ModelCache.InitProgress):
                    OnPropertyChanged(nameof(InitProgress));
                    break;
            }
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Init()
        {
            InitProgress = 0.0;
            Boolean success = await UploadRecentItems();

            InitProgress = 1.0;
            if (success)
            {
                await _modelCache.Init();
            }

            UpdateAvailableUnits();

            InitProgress = 2.0;
            UpdateAvailableIngredients();

            InitProgress = 3.0;
            UpdateRecipes();

            InitProgress = 4.0;
            UpdateDrinks();

            InitProgress = 5.0;
        }

        public async Task<Boolean> UploadRecentItems()
        {
            return await _modelCache.UploadRecentItems();
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

        internal void Save()
        {
            _modelCache.Save();
        }

        public void Dispose()
        {
            _modelCache.Dispose();
        }

        public async Task<Boolean> CreateRecipe(RecipeViewModel viewModel, Recipe model)
        {
            Boolean result = await _modelCache.CreateRecipe(model);

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

        public async Task AddPicture(Recipe model, String path)
        {
            await _modelCache.AddPicture(model, path);
        }

        public async Task UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            await _modelCache.UpdateFavoriteDrink(drink, isFavorite);
        }
    }
}
