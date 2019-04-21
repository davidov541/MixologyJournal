﻿using System;
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
    public sealed partial class SettingsPage : Page
    {
        private ISettingsPageViewModel _viewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _viewModel = e.Parameter as ISettingsPageViewModel;
            DataContext = _viewModel;
        }

        private async void ClearCache_Click(Object sender, RoutedEventArgs e)
        {
            await _viewModel.ClearCacheAsync();
        }
    }
}
