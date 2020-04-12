using MixologyJournalApp.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using MixologyJournalApp.ViewModel;

namespace MixologyJournalApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;

        public MainPage()
        {
            _viewModel = new MainPageViewModel();
            InitializeComponent();
        }

        private async Task RefreshItems()
        {
            this.loginButton.IsVisible = !_viewModel.IsAuthenticated;
            await _viewModel.UpdateRecipes();
            IEnumerable<String> recipeNames = _viewModel.Recipes.Select(r => r.Name);
            messageLabel.Text = String.Join(",", recipeNames);
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.LogIn();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (_viewModel.IsAuthenticated)
            {
                await RefreshItems();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (_viewModel.IsAuthenticated)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems();

                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;
            }
        }
    }
}
