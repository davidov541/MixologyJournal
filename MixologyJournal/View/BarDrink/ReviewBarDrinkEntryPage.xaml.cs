using System;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.Entry;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MixologyJournal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewBarDrinkEntryPage : Page
    {
        private IBarDrinkEntryViewModel _viewModel;
		/*
        public BaseAppPersister Persister
        {
            get
            {
                return _viewModel.App.Persister;
            }
        }
		*/

        public ReviewBarDrinkEntryPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = e.Parameter as IBarDrinkEntryViewModel;
            DataContext = _viewModel;
            _viewModel.App.NearbyPlaces.GetNearbyPlacesAsync().ContinueWith(t =>
            {
                LocationsBox.IsEnabled = true;
                LocationsBox.ItemsSource = t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewModel.Cancel();
            await _viewModel.DeleteAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private async void SubmitButton_Click(Object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveAsync();

            Frame.Navigate(typeof(BarDrinkEntryViewPage), _viewModel);
        }

        private async void TakePictureButon_Click(Object sender, RoutedEventArgs e)
        {
            await _viewModel.AddNewPictureAsync();
        }

        private async void AddPictureButon_Click(Object sender, RoutedEventArgs e)
        {
            await _viewModel.AddExistingPictureAsync();
        }
    }
}
