using System;
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
    public sealed partial class EditBarDrinkEntryPage : Page
    {
        private IBarDrinkEntryViewModel _viewModel;

        public EditBarDrinkEntryPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = e.Parameter as IBarDrinkEntryViewModel;
            DataContext = _viewModel;
        }

        private async void SubmitButton_Click(Object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveAsync();
            Frame.Navigate(typeof(ReviewBarDrinkEntryPage), _viewModel);
        }

        private async void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewModel.Cancel();
            await _viewModel.DeleteAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private void AddIngredButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewModel.AddIngredient();
        }
    }
}
