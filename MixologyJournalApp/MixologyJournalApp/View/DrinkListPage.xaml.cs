using MixologyJournalApp.View.Controls;
using MixologyJournalApp.ViewModel;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    [DesignTimeVisible(false)]
    public partial class DrinkListPage : ContentPage
    {
        private readonly DrinkListPageViewModel _viewModel;
        private readonly App _app;
        private readonly ICommand _selectionCommand;

        public DrinkListPage(App app)
        {
            _app = app;
            _viewModel = new DrinkListPageViewModel(_app);
            BindingContext = _viewModel;
            _selectionCommand = new Command(ItemSelected);

            InitializeComponent();

            UpdateDrinkList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateDrinkList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void UpdateDrinkList()
        {
            RecipeListLayout.Children.Clear();
            foreach (DrinkViewModel drink in _viewModel.Drinks)
            {
                RecipeListLayout.Children.Add(new SummaryCardView(drink, _selectionCommand));
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DrinkListPageViewModel.Drinks):
                    UpdateDrinkList();
                    break;
            }
        }

        private async void ItemSelected(object item)
        {
            DrinkPage drinkPage = new DrinkPage(item as DrinkViewModel);
            await Navigation.PushAsync(drinkPage);
        }

        private async void AddDrinkButton_Clicked(object sender, EventArgs e)
        {
            CreateDrinkPage drinkPage = new CreateDrinkPage(_app);
            await Navigation.PushAsync(drinkPage);
        }
    }
}
