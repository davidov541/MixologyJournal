using MixologyJournalApp.MAUI.Data;
using MixologyJournalApp.MAUI.Model;
using MixologyJournalApp.MAUI.ViewModel;
using System.Collections.ObjectModel;

namespace MixologyJournalApp.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        private RecipeListPageViewModel _viewModel;

        public MainPage(AppViewModel viewModel)
        {
            InitializeComponent();
            this._viewModel = new RecipeListPageViewModel(viewModel);
            BindingContext = this._viewModel;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            await this._viewModel.InitializeAsync();
        }
    }
}