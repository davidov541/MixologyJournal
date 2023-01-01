using MixologyJournalApp.MAUI.ViewModel;


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
            this.RecipeList.SelectedItem = null;
            await this._viewModel.InitializeAsync();
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not RecipeViewModel recipe)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(RecipeViewPage), true, new Dictionary<string, object>
            {
                ["Recipe"] = recipe
            });
        }
    }
}