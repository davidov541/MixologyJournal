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
    public sealed partial class BaseRecipePageEdit : Page
    {
        private IBaseRecipeEditPageViewModel _recipe;
        public BaseRecipePageEdit()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _recipe = e.Parameter as IBaseRecipeEditPageViewModel;
            DataContext = _recipe;
        }

        private async void SubmitButton_Click(Object sender, RoutedEventArgs e)
        {
            await _recipe.SaveAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private async void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            _recipe.Cancel();
            await _recipe.DeleteAsync();
            Frame.Navigate(typeof(OverviewPage));
        }

        private void AddIngredButton_Click(Object sender, RoutedEventArgs e)
        {
            _recipe.AddIngredient();
        }
    }
}
