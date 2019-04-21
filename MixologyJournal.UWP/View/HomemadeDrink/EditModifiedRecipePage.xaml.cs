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
    public sealed partial class EditModifiedRecipePage : Page
    {
        private IHomemadeDrinkEntryViewModel ViewModel
        {
            get
            {
                return DataContext as IHomemadeDrinkEntryViewModel;
            }
        }

        public EditModifiedRecipePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = e.Parameter as IHomemadeDrinkEntryViewModel;
        }

        private async void NextButton_Click(Object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();

            Frame.Navigate(typeof(ModifiedRecipeReviewPage), ViewModel);
        }

        private void AddIngredButton_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.AddIngredient();
        }

        private async void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
            await ViewModel.DeleteAsync();
            Frame.Navigate(typeof(OverviewPage));
        }
    }
}
