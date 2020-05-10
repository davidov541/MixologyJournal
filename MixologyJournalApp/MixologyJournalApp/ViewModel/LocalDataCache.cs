using MixologyJournalApp.Model;
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
            await UpdateRecipes();
            InitProgress = 0.75;
            await UpdateDrinks();
            InitProgress = 1.0;
        }

        public async Task Resync()
        {
            await Init();
        }

        private async Task UpdateRecipes()
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/recipes");
            List<RecipeViewModel> recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult).Select(r => new RecipeViewModel(r, _app)).ToList();
            Recipes.Clear();
            foreach (RecipeViewModel r in recipes.OrderBy(i => i.Name))
            {
                Recipes.Add(r);
            }
        }

        private async Task UpdateDrinks()
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/drinks");
            List<DrinkViewModel> drinks = JsonConvert.DeserializeObject<List<Drink>>(jsonResult).Select(d => new DrinkViewModel(d, _app)).ToList();
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

        public void CreateRecipe(RecipeViewModel recipe)
        {
            RecipeViewModel insertBeforeRecipe = Recipes.FirstOrDefault(r => recipe.Name.CompareTo(r.Name) < 0);
            if (insertBeforeRecipe == null)
            {
                Recipes.Add(recipe);
            } 
            else
            {
                int insertIndex = Recipes.IndexOf(insertBeforeRecipe);
                Recipes.Insert(insertIndex, recipe);
            }
        }

        public void DeleteRecipe(RecipeViewModel recipe)
        {
            Recipes.Remove(recipe);
        }

        public void CreateDrink(DrinkViewModel drink)
        {
            DrinkViewModel insertBeforeRecipe = Drinks.FirstOrDefault(d => drink.Name.CompareTo(d.Name) < 0);
            if (insertBeforeRecipe == null)
            {
                Drinks.Add(drink);
            }
            else
            {
                int insertIndex = Drinks.IndexOf(insertBeforeRecipe);
                Drinks.Insert(insertIndex, drink);
            }
        }
    }
}
