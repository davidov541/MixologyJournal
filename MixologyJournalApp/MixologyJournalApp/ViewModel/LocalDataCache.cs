using MixologyJournalApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class LocalDataCache
    {
        private readonly App _app;

        public ObservableCollection<RecipeViewModel> Recipes { get; } = new ObservableCollection<RecipeViewModel>();

        public ObservableCollection<IngredientViewModel> AvailableIngredients { get; } = new ObservableCollection<IngredientViewModel>();

        public ObservableCollection<UnitViewModel> AvailableUnits { get; } = new ObservableCollection<UnitViewModel>();

        public LocalDataCache(App app)
        {
            _app = app;
        }

        public async Task Init()
        {
            await UpdateRecipes();
            await UpdateAvailableIngredients();
            await UpdateAvailableUnits();
        }

        public async Task Resync()
        {
            await UpdateRecipes();
            await UpdateAvailableIngredients();
            await UpdateAvailableUnits();
        }

        private async Task UpdateRecipes()
        {
            String jsonResult = await _app.PlatformInfo.Backend.GetResult("/insecure/recipes");
            List<RecipeViewModel> recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult).Select(r => new RecipeViewModel(r)).ToList();
            Recipes.Clear();
            foreach (RecipeViewModel r in recipes.OrderBy(i => i.Name))
            {
                Recipes.Add(r);
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
    }
}
