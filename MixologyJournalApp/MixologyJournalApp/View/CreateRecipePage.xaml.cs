using MixologyJournalApp.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateRecipePage : ContentPage
    {
        private readonly RecipeViewModel _vm;
        private readonly App _app;

        internal CreateRecipePage(App app, RecipeViewModel recipe)
        {
            _vm = recipe;
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
                    "We were unable to add the recipe to the server. The drink is saved in the local cache, and will be uploaded once possible.");
            }
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            IngredientUsageViewModel viewModel = (sender as Button).BindingContext as IngredientUsageViewModel;
            await Navigation.PushModalAsync(new ModifyIngredientPage(viewModel), true);
        }
    }
}