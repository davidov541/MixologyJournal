﻿using MixologyJournalApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage : ContentPage
    {
        private RecipeViewModel _viewModel;

        internal RecipePage(RecipeViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
            InitializeComponent();
        }
    }
}