using MixologyJournalApp.Model;
using MixologyJournalApp.View;
using System;
using Xamarin.Forms;

namespace MixologyJournalApp
{
    public partial class App : Application
    {
        private static App _instance;

        public IBackend Backend { get; private set; }

        public IAlertDialogFactory DialogFactory { get; private set; }

        public static App GetInstance(IBackend authenticator, IAlertDialogFactory dialogFactory)
        {
            if (_instance == null)
            {
                _instance = new App(authenticator, dialogFactory);
            }
            return _instance;
        }

        public static App GetInstance()
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("GetInstance called without any parameters when no instance was available yet.");
            }
            return _instance;
        }

        private App(IBackend authenticator, IAlertDialogFactory dialogFactory)
        {
            Backend = authenticator;
            DialogFactory = dialogFactory;

            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
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
