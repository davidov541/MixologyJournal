using System;
using MixologyJournal.ViewModel.Entry;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MixologyJournal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BaseRecipePageView : Page
    {
        private IBaseRecipePageViewModel _recipe;
        public BaseRecipePageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _recipe = e.Parameter as IBaseRecipePageViewModel;
            DataContext = _recipe;
        }

        private void RelatedRecipes_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            EntryList list = sender as EntryList;
            IRecipePageViewModel recipe = list.SelectedItem as IRecipePageViewModel;
            Frame.Navigate(typeof(ModifiedRecipeEntryViewPage), recipe);
        }

        private async void DeleteButton_Click(Object sender, RoutedEventArgs e)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            ContentDialog areYouSureDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("DeleteDialogHeader"),
                Content = resourceLoader.GetString("DeleteBaseRecipeDialogDescription"),
                PrimaryButtonText = resourceLoader.GetString("YesOption"),
                SecondaryButtonText = resourceLoader.GetString("NoOption")
            };

            ContentDialogResult result = await areYouSureDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await _recipe.DeleteAsync();
                Frame.Navigate(typeof(OverviewPage));
            }
        }

        private async void MakeDrinkButton_Click(Object sender, RoutedEventArgs e)
        {
            IHomemadeDrinkEntryViewModel entry = await _recipe.Recipe.CreateModifiedRecipeEntryAsync(_recipe);
            Frame.Navigate(typeof(EditModifiedRecipePage), entry);
        }
    }
}
