using MixologyJournalApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class MainPageViewModel
    {
        public bool IsAuthenticated
        {
            get
            {
                return App.GetInstance().Backend.IsAuthenticated;
            }
        }

        private List<RecipeViewModel> _recipes = new List<RecipeViewModel>();
        public IEnumerable<RecipeViewModel> Recipes
        {
            get
            {
                return _recipes;
            }
        }

        public MainPageViewModel()
        {
        }

        public async Task UpdateRecipes()
        {
            if (IsAuthenticated)
            {
                String jsonResult = await App.GetInstance().Backend.GetResult("/insecure/recipes");
                _recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult).Select(r => new RecipeViewModel(r)).ToList();
            }
            else
            {
                _recipes = new List<RecipeViewModel>();
            }
        }

        public async Task LogIn()
        {
            if (App.GetInstance().Backend != null)
            {
                if (await App.GetInstance().Backend.Authenticate())
                {
                    // Display the success or failure message.
                    String message = string.Format("you are now signed-in as {0}.", App.GetInstance().Backend.User.UserId); ;
                    App.GetInstance().DialogFactory.showDialog("Sign-in result", message);
                }
            }
        }
    }
}
