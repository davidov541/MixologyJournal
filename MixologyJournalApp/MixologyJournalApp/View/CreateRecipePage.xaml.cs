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

        internal CreateRecipePage(RecipeViewModel recipe)
        {
            _vm = recipe;
            BindingContext = _vm;
            InitializeComponent();
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            bool result = await _vm.SaveNew();
            if (result)
            {
                await Navigation.PopAsync();
            }
            else
            {
                App.GetInstance().PlatformInfo.AlertDialogFactory.ShowDialog("Save Failed", "Could not save the new recipe. Please try again.");
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

        private void AddIngredientButton_Clicked(object sender, EventArgs e)
        {
            _vm.AddIngredient();
        }

        private void DeleteIngredientButton_Clicked(object sender, EventArgs e)
        {
            IngredientUsageViewModel vm = (sender as Button).BindingContext as IngredientUsageViewModel;
            _vm.DeleteIngredient(vm);
        }
    }
}