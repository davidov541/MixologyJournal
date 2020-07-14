﻿using MixologyJournalApp.View.Controls;
using MixologyJournalApp.ViewModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage : ContentPage
    {
        private readonly RecipeViewModel _viewModel;
        private readonly App _app;

        private ICommand DrinkSelectedCommand;

        internal RecipePage(App app, RecipeViewModel viewModel)
        {
            _viewModel = viewModel;
            _app = app;
            BindingContext = _viewModel;
            InitializeComponent();

            RecipeList.Children.Add(new DetailCardView(_viewModel, null));

            DrinkSelectedCommand = new Command(DrinkSelected);
            foreach (DrinkViewModel recipe in _viewModel.AssociatedDrinks)
            {
                DrinkList.Children.Add(new DetailCardView(recipe, DrinkSelectedCommand));
            }
        }

        private async void DrinkSelected(object parameter)
        {
            DrinkViewModel drink = parameter as DrinkViewModel;

            DrinkPage drinkPage = new DrinkPage(drink);
            await Navigation.PushAsync(drinkPage);
        }

        private async void AddDrinkButton_Clicked(object sender, System.EventArgs e)
        {
            CreateDrinkPage drinkPage = new CreateDrinkPage(_app, _viewModel);
            await Navigation.PushAsync(drinkPage);
        }
    }
}