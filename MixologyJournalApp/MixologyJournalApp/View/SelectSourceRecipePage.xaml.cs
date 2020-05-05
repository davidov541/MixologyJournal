using MixologyJournalApp.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectSourceRecipePage : ContentPage
    {
        private readonly SelectSourceRecipePageViewModel _vm;
        private readonly App _app;

        internal SelectSourceRecipePage(App app)
        {
            _vm = new SelectSourceRecipePageViewModel(app);
            _app = app;
            BindingContext = _vm;
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CreateDrinkPage drinkPage = new CreateDrinkPage(_app, e.Item as RecipeViewModel);
            await Navigation.PushAsync(drinkPage);
        }
    }
}