using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MixologyJournalApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            this.loginButton.IsVisible = !App.Authenticator.IsAuthenticated;
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
            {
                if (await App.Authenticator.Authenticate())
                {
                    // Display the success or failure message.
                    String message = string.Format("you are now signed-in as {0}.", App.Authenticator.User.UserId); ;
                    App.DialogFactory.showDialog("Sign-in result", message);
                }
            }

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (App.Authenticator.IsAuthenticated)
            {
                await RefreshItems(true, syncItems: false);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (App.Authenticator.IsAuthenticated)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;
            }
        }
    }
}
