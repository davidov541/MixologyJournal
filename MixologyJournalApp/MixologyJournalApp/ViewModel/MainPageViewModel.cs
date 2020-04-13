using MixologyJournalApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class MainPageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAuthenticated
        {
            get
            {
                bool authenticated = false;
                try
                {
                    authenticated = App.GetInstance().Backend.IsAuthenticated;
                } catch (InvalidOperationException)
                {
                    // We haven't created the App object yet, so we should just return that we aren't authenticated.
                }
                return authenticated;
            }
        }

        public bool IsUnauthenticated
        {
            get
            {
                return !IsAuthenticated;
            }
        }

        public String Message
        {
            get
            {
                IEnumerable<String> recipeNames = Recipes.Select(r => r.Name);
                return String.Join(",", recipeNames);
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
                _recipes.ForEach(r => r.PropertyChanged += Recipe_PropertyChanged);
                OnPropertyChanged(nameof(Recipes));
                OnPropertyChanged(nameof(Message));
            }
            else
            {
                _recipes = new List<RecipeViewModel>();
            }
        }

        private void Recipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Recipes));
            OnPropertyChanged(nameof(Message));
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
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsUnauthenticated));
        }

        public async Task LogOff()
        {
            if (App.GetInstance().Backend != null)
            {
                await App.GetInstance().Backend.LogOffAsync();
                App.GetInstance().DialogFactory.showDialog("Sign-out result", "Logged out");
            }
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsUnauthenticated));
        }
    }
}
