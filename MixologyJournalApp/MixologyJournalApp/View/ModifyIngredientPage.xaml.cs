using MixologyJournalApp.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyIngredientPage : ContentPage
    {
        private ModifyIngredientPageViewModel _viewModel;

        internal ModifyIngredientPage(IngredientUsageViewModel viewModel, App currentApp)
        {
            _viewModel = new ModifyIngredientPageViewModel(viewModel, currentApp);
            BindingContext = _viewModel;
            _viewModel.SaveCurrentState();

            InitializeComponent();
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.RestoreFromState();
            await Navigation.PopModalAsync(true);
        }
    }
}