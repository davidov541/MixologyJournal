using MixologyJournalApp.ViewModel;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateDrinkPage : ContentPage
    {
        private readonly DrinkViewModel _vm;
        private readonly App _app;

        internal CreateDrinkPage(App app, RecipeViewModel basis)
        {
            _vm = new DrinkViewModel(basis, app);
            _app = app;
            BindingContext = _vm;
            InitializeComponent();
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            bool result = await _vm.SaveNew();
            if (!result)
            {
                _app.PlatformInfo.AlertDialogFactory.ShowDialog("Server Unavailable",
                    "We were unable to add the drink to the server. The drink is saved in the local cache, and will be uploaded once possible.");
            }
            if (Navigation.ModalStack.Any())
            {
                // We created the drink from the root page, in which case the basis recipe choice screen was modal and we need to handle that.
                await Navigation.PopModalAsync();
            }
            else
            {
                // We created the drink from the recipe page, so we didn't have any modal pages to deal with.
                await Navigation.PopToRootAsync();
            }
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