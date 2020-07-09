using MixologyJournalApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrinkPage : ContentPage
    {
        private DrinkViewModel _viewModel;

        internal DrinkPage(DrinkViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
            InitializeComponent();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.SaveChanges();
        }
    }
}