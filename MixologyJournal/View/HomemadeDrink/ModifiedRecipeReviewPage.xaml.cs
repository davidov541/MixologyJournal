using System;
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
    public sealed partial class ModifiedRecipeReviewPage : Page
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

        public ModifiedRecipeReviewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = e.Parameter as IHomemadeDrinkEntryViewModel;
        }

        private async void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
            await ViewModel.DeleteAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private async void SubmitButton_Click(Object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();

            Frame.Navigate(typeof(ModifiedRecipeEntryViewPage), ViewModel);
        }

        private async void TakePictureButon_Click(Object sender, RoutedEventArgs e)
        {
            await ViewModel.AddNewPictureAsync();
        }

        private async void AddPictureButon_Click(Object sender, RoutedEventArgs e)
        {
            await ViewModel.AddExistingPictureAsync();
        }
    }
}
