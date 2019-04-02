using System;
using MixologyJournal.Persistence;
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
    public sealed partial class ModifiedRecipeEntryViewPage : Page
    {
		/*
        public BaseAppPersister Persister
        {
            get
            {
                return ViewModel.App.Persister;
            }
        }
		*/

        private IHomemadeDrinkEntryViewModel ViewModel
        {
            get
            {
                return DataContext as IHomemadeDrinkEntryViewModel;
            }
        }

        public ModifiedRecipeEntryViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = e.Parameter as IHomemadeDrinkEntryViewModel;
        }

        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverviewPage));
        }

        private async void FavoriteButton_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.IsFavorite = !ViewModel.IsFavorite;
            await ViewModel.SaveAsync();
        }

        private void RelatedRecipes_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            EntryList list = sender as EntryList;
            IRecipePageViewModel recipe = list.SelectedItem as IRecipePageViewModel;
            Frame.Navigate(typeof(ModifiedRecipeEntryViewPage), recipe);
        }

        private async void DeleteEntryButton_Click(Object sender, RoutedEventArgs e)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            ContentDialog areYouSureDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("DeleteDialogHeader"),
                Content = resourceLoader.GetString("DeleteJournalEntryDialogDescription"),
                PrimaryButtonText = resourceLoader.GetString("YesOption"),
                SecondaryButtonText = resourceLoader.GetString("NoOption")
            };

            ContentDialogResult result = await areYouSureDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await ViewModel.DeleteAsync();
                Frame.Navigate(typeof(OverviewPage));
            }
        }
    }
}
