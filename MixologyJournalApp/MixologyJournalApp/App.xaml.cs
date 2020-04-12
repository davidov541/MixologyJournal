using MixologyJournalApp.Security;
using MixologyJournalApp.View;
using Xamarin.Forms;

namespace MixologyJournalApp
{
    public partial class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }

        public static IAlertDialogFactory DialogFactory { get; private set; }

        public static void Init(IAuthenticate authenticator, IAlertDialogFactory dialogFactory)
        {
            Authenticator = authenticator;
            DialogFactory = dialogFactory;
        }

        public App()
        {
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
