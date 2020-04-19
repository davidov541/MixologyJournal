using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using MixologyJournalApp.View;
using MixologyJournalApp.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MixologyJournalApp
{
    public partial class App : Application
    {
        private static App _instance;

        public IPlatform PlatformInfo { get; private set; }

        public static App GetInstance(IPlatform platform)
        {
            if (_instance == null)
            {
                _instance = new App(platform);
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

        private LoadingPage _loadingPage;
        private App(IPlatform platform)
        {
            PlatformInfo = platform;

            InitializeComponent();

            _loadingPage = new LoadingPage();
            MainPage = _loadingPage;
        }

        public async Task LoadAsync()
        {
            MainPageViewModel pageVM = await _loadingPage.LoadAsync();
            MainPage = new NavigationPage(new MainPage(pageVM));
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
