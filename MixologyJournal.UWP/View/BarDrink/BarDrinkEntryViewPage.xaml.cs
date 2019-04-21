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
    public sealed partial class BarDrinkEntryViewPage : Page
    {
		/*
        public BaseAppPersister Persister
        {
            get
            {
                return _viewModel.App.Persister;
            }
        }
		*/

        private IBarDrinkEntryViewModel _viewModel;
        public BarDrinkEntryViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = e.Parameter as IBarDrinkEntryViewModel;
            DataContext = _viewModel;
        }

        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverviewPage));
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
                await _viewModel.DeleteAsync();
                Frame.Navigate(typeof(OverviewPage));
            }
        }
    }
}
