using System;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.Entry;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MixologyJournal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TextJournalEntryViewPage : Page
    {
        private INotesEntryViewModel _entry;
		/*
        public BaseAppPersister Persister
        {
            get
            {
                return _entry.App.Persister;
            }
        }
		*/

        public TextJournalEntryViewPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            INotesEntryViewModel viewModel = e.Parameter as INotesEntryViewModel;
            _entry = viewModel;
            DataContext = _entry;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        private void GoBackButton_Click(Object sender, RoutedEventArgs e)
        {
            _entry.Cancel();
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
                await _entry.DeleteAsync();
                Frame.Navigate(typeof(OverviewPage));
            }
        }
    }
}
