using MixologyJournalApp.ViewModel;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DrinkListPage : ContentPage
    {
        private readonly DrinkListPageViewModel _viewModel;
        private readonly App _app;

        public DrinkListPage(App app)
        {
            _app = app;
            _viewModel = new DrinkListPageViewModel(_app);
            BindingContext = _viewModel;
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RecipePage recipePage = new RecipePage(e.Item as RecipeViewModel);
            await Navigation.PushAsync(recipePage);
        }

        private async void AddDrinkButton_Clicked(object sender, EventArgs e)
        {
            CreateRecipePage recipePage = new CreateRecipePage(_app, new RecipeViewModel(_app));
            await Navigation.PushAsync(recipePage);
        }
    }
}
