using System;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MixologyJournal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : Page
    {
        private MixologyApp _app;
        public LoadingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _app = e.Parameter as MixologyApp;
            _app.InitializeAsync().ContinueWith(t =>
            {
                NavigateToOverview();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void NavigateToOverview()
        {
            Frame.Navigate(typeof(OverviewPage), _app.GetOverviewPage());
            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Frame.BackStack.Remove(Frame.BackStack.LastOrDefault());
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
    }
}
