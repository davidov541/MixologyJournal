using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.List;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MixologyJournal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverviewPage : Page, INotifyPropertyChanged
    {
        private static IOverviewPageViewModel _viewModel;

        private IEnumerable<IListEntry> _wideListSource;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<IListEntry> WideListSource
        {
            get
            {
                return _wideListSource;
            }
            private set
            {
                _wideListSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WideListSource)));
            }
        }

        public OverviewPage()
        {
            _wideListSource = new ObservableCollection<IListEntry>();

            this.InitializeComponent();

            SizeChanged += Page_SizeChanged;

            ResourceLoader resourceLoader = new ResourceLoader();
            NavList.Items.Add(new NavMenuItem(String.Empty, Symbol.List, ToggleNavExpansion));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("RecipesLabel"), Symbol.Admin, ViewRecipes));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("DrinksLabel"), Symbol.Admin, ViewDrinks));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("NotesLabel"), Symbol.Admin, ViewNotes));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("AddRecipeLabel"), Symbol.Add, null));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("AddHomemadeDrinkLabel"), Symbol.Add, null));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("AddBarDrinkLabel"), Symbol.Add, null));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("AddNoteLabel"), Symbol.Add, null));
            NavList.Items.Add(new NavMenuItem(resourceLoader.GetString("SettingsLabel"), Symbol.Setting, null));

            UpdateView();
        }

        private void Page_SizeChanged(Object sender, SizeChangedEventArgs e)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            if (ActualWidth > 750)
            {
                NavView.Visibility = Visibility.Visible;
                BottomAppBar.Visibility = Visibility.Collapsed;
                NarrowView.Visibility = Visibility.Collapsed;
            }
            else
            {
                NavView.Visibility = Visibility.Collapsed;
                BottomAppBar.Visibility = Visibility.Visible;
                NarrowView.Visibility = Visibility.Visible;
            }
        }

        private void ToggleNavExpansion()
        {
            NavView.IsPaneOpen = !NavView.IsPaneOpen;
        }

        private void ViewRecipes()
        {
            WideListSource = _viewModel.Recipes;
        }

        private void ViewNotes()
        {
            WideListSource = _viewModel.Entries.OfType<INotesEntryViewModel>();
        }

        private void ViewDrinks()
        {
            WideListSource = _viewModel.Entries.OfType<IDrinkEntryViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_viewModel == null)
            {
                _viewModel = e.Parameter as IOverviewPageViewModel;
            }
            DataContext = _viewModel;
            ViewRecipes();
        }

        private async void AddTextEntryButton_Click(Object sender, RoutedEventArgs e)
        {
            INotesEntryViewModel textJournal = await _viewModel.CreateTextJournalEntryAsync();
            Frame.Navigate(typeof(TextJournalEntryCreationPage), textJournal);
        }

        private void AddModifiedRecipeEntryButton_Click(Object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BaseRecipeSelectionPage), _viewModel);
        }

        private async void AddRecipeButton_Click(Object sender, RoutedEventArgs e)
        {
            IBaseRecipePageViewModel recipe = await _viewModel.CreateBaseRecipeAsync();
            Frame.Navigate(typeof(BaseRecipePageEdit), recipe);
        }

        private void JournalEntries_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            EntryList list = sender as EntryList;
            INotesEntryViewModel entry = list.SelectedItem as INotesEntryViewModel;
            Frame.Navigate(typeof(TextJournalEntryViewPage), entry);
        }

        private void Recipes_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(BaseRecipePageView), (sender as EntryList).SelectedItem);
        }

        private void SettingsButton_Click(Object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage), _viewModel.CreateSettingsPage());
        }

        private async void AddBarDrink_Click(Object sender, RoutedEventArgs e)
        {
            IBarDrinkEntryViewModel viewModel = await _viewModel.CreateBarDrinkPageAsync();
            Frame.Navigate(typeof(EditBarDrinkEntryPage), viewModel);
        }

        private void DrinksList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            EntryList list = sender as EntryList;
            ISaveablePageViewModel entry = list.SelectedItem as ISaveablePageViewModel;
            if (entry is IHomemadeDrinkEntryViewModel)
            {
                Frame.Navigate(typeof(ModifiedRecipeEntryViewPage), entry);
            }
            else if (entry is IBarDrinkEntryViewModel)
            {
                Frame.Navigate(typeof(BarDrinkEntryViewPage), entry);
            }
        }

        private void NavList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                (e.AddedItems[0] as NavMenuItem).Execute();
                NavList.SelectedItem = null;
            }
        }

        private void WideList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
