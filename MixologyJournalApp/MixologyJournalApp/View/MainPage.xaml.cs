﻿using MixologyJournalApp.ViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;

        internal MainPage(MainPageViewModel vm)
        {
            _viewModel = vm;
            BindingContext = _viewModel;
            InitializeComponent();
        }

        private async Task RefreshItems()
        {
            await _viewModel.UpdateRecipes();
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
            }
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.LogIn();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (_viewModel.IsAuthenticated)
            {
                await RefreshItems();
            }
        }

        private async void logoutButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.LogOff();
        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RecipePage recipePage = new RecipePage(e.Item as RecipeViewModel);
            await Navigation.PushAsync(recipePage);
        }

        private async void addRecipeButton_Clicked(object sender, EventArgs e)
        {
            CreateRecipePage recipePage = new CreateRecipePage(new RecipeViewModel());
            await Navigation.PushAsync(recipePage);
        }
    }
}
