using System;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.Entry;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MixologyJournal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TextJournalEntryCreationPage : Page
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

        public TextJournalEntryCreationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _entry = e.Parameter as INotesEntryViewModel;
            DataContext = _entry;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        private async void SubmitButton_Click(Object sender, RoutedEventArgs e)
        {
            await _entry.SaveAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private async void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            _entry.Cancel();
            await _entry.DeleteAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private async void TakePictureButon_Click(Object sender, RoutedEventArgs e)
        {
            await _entry.AddNewPictureAsync();
        }

        private async void AddPictureButon_Click(Object sender, RoutedEventArgs e)
        {
            await _entry.AddExistingPictureAsync();
        }
    }
}
