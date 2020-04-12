using MixologyJournalApp.Security;
using MixologyJournalApp.View;
using Xamarin.Forms;

namespace MixologyJournalApp
{
    public partial class App : Application
    {
        private static App _instance;

        public static IAuthenticate Authenticator { get; private set; }

        public static IAlertDialogFactory DialogFactory { get; private set; }

        public static App GetInstance(IAuthenticate authenticator, IAlertDialogFactory dialogFactory)
        {
            if (_instance == null)
            {
                _instance = new App(authenticator, dialogFactory);
            }
            return _instance;
        }

        private App(IAuthenticate authenticator, IAlertDialogFactory dialogFactory)
        {
            Authenticator = authenticator;
            DialogFactory = dialogFactory;

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
