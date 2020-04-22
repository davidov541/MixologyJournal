using MixologyJournalApp.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateRecipePage : ContentPage
    {
        private RecipeViewModel _vm;

        internal CreateRecipePage(RecipeViewModel recipe)
        {
            _vm = recipe;
            BindingContext = _vm;
            InitializeComponent();
        }

        private async void createButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void addStepButton_Clicked(object sender, EventArgs e)
        {
            _vm.AddStep();
        }

        private void deleteStepButton_Clicked(object sender, EventArgs e)
        {
            StepViewModel vm = (sender as Button).BindingContext as StepViewModel;
            _vm.DeleteStep(vm);
        }

        private void addIngredientButton_Clicked(object sender, EventArgs e)
        {
            _vm.AddIngredient();
        }

        private void deleteIngredientButton_Clicked(object sender, EventArgs e)
        {
            IngredientUsageViewModel vm = (sender as Button).BindingContext as IngredientUsageViewModel;
            _vm.DeleteIngredient(vm);
        }
    }
}