using MixologyJournalApp.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateDrinkPage : ContentPage
    {
        private readonly App _app;
        private DrinkViewModel _vm;
        private SelectSourceRecipePage _selectPage;

        internal CreateDrinkPage(App app, RecipeViewModel basis)
        {
            _vm = new DrinkViewModel(basis, app);
            _app = app;
            BindingContext = _vm;
            InitializeComponent();
        }

        internal CreateDrinkPage(App app)
        {
            _app = app;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_vm == null && _selectPage == null)
            {
                _selectPage = new SelectSourceRecipePage(_app);
                _selectPage.BasisChanged += SelectPage_BasisChanged;
                await Navigation.PushModalAsync(new NavigationPage(_selectPage));
            }
        }

        private void SelectPage_BasisChanged(object sender, SelectSourceRecipePage.BasisChangedEventArgs e)
        {
            _vm = new DrinkViewModel(e.NewBasis, _app);
            BindingContext = _vm;
            _selectPage.BasisChanged -= SelectPage_BasisChanged;
            _selectPage = null;
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            bool result = await _vm.SaveNew();
            if (!result)
            {
                _app.PlatformInfo.AlertDialogFactory.ShowDialog("Server Unavailable",
                    "We were unable to add the drink to the server. The drink is saved in the local cache, and will be uploaded once possible.");
            }
            await Navigation.PopToRootAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void AddStepButton_Clicked(object sender, EventArgs e)
        {
            _vm.AddStep();
        }

        private void DeleteStepButton_Clicked(object sender, EventArgs e)
        {
            StepViewModel vm = (sender as Button).BindingContext as StepViewModel;
            _vm.DeleteStep(vm);
        }

        private async void ModifyIngredientButton_Clicked(object sender, EventArgs e)
        {
            IngredientUsageViewModel viewModel = (sender as ImageButton).BindingContext as IngredientUsageViewModel;
            await Navigation.PushModalAsync(new ModifyIngredientPage(viewModel), true);
        }
    }
}