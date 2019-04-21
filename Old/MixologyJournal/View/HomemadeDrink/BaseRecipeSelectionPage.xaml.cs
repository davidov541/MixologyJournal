using System;
using System.Collections.Generic;
using MixologyJournal.ViewModel;
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
    public sealed partial class BaseRecipeSelectionPage : Page
    {
        private IOverviewPageViewModel _viewModel;

        public BaseRecipeSelectionPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _viewModel = e.Parameter as IOverviewPageViewModel;
            DataContext = _viewModel;
        }

        private async void BaseRecipeSelector_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            BusyGrid.Visibility = Visibility.Visible;
            IBaseRecipePageViewModel recipe = BaseRecipeList.SelectedItem as IBaseRecipePageViewModel;
            IHomemadeDrinkEntryViewModel viewModel = await recipe.Recipe.CreateModifiedRecipeEntryAsync(recipe);
            BusyGrid.Visibility = Visibility.Collapsed;

            Frame.Navigate(typeof(EditModifiedRecipePage), viewModel);
        }

        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverviewPage));
        }
    }
}
