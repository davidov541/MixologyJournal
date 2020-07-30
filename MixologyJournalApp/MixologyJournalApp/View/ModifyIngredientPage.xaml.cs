using MixologyJournalApp.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyIngredientPage : ContentPage
    {
        private IngredientUsageViewModel _viewModel;
        private IngredientUsageViewModel.State _previousState;

        internal ModifyIngredientPage(IngredientUsageViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
            _previousState = new IngredientUsageViewModel.State(_viewModel);

            InitializeComponent();
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.RestoreFromState(_previousState);
            await Navigation.PopModalAsync(true);
        }
    }
}