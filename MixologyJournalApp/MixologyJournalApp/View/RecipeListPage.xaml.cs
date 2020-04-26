using MixologyJournalApp.ViewModel;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RecipeListPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public RecipeListPage()
        {
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RecipePage recipePage = new RecipePage(e.Item as RecipeViewModel);
            await Navigation.PushAsync(recipePage);
        }

        private async void AddRecipeButton_Clicked(object sender, EventArgs e)
        {
            CreateRecipePage recipePage = new CreateRecipePage(new RecipeViewModel());
            await Navigation.PushAsync(recipePage);
        }
    }
}
