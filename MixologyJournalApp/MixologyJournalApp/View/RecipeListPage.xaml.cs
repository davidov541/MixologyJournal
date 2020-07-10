﻿using MixologyJournalApp.ViewModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RecipeListPage : ContentPage
    {
        private readonly RecipeListPageViewModel _viewModel;
        private readonly App _app;
        private readonly ICommand _selectionCommand;

        public RecipeListPage(App app)
        {
            _app = app;
            _viewModel = new RecipeListPageViewModel(_app);
            BindingContext = _viewModel;
            _selectionCommand = new Command(ItemSelected);

            InitializeComponent();

            foreach (RecipeViewModel recipe in _viewModel.Recipes)
            {
                RecipeListLayout.Children.Add(new CardView(recipe, _selectionCommand));
            }
        }

        private async void ItemSelected(object item)
        {
            RecipePage recipePage = new RecipePage(item as RecipeViewModel);
            await Navigation.PushAsync(recipePage);
        }

        private async void AddRecipeButton_Clicked(object sender, EventArgs e)
        {
            CreateRecipePage recipePage = new CreateRecipePage(_app, new RecipeViewModel(_app));
            await Navigation.PushAsync(recipePage);
        }
    }
}
