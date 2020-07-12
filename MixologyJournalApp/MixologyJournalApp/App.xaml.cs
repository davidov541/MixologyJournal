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
        private const String _hasBeenSetupKey = "HasBeenSetUp";

        public IPlatform PlatformInfo { get; private set; }

        internal LocalDataCache Cache { get; private set; }

        public static App Create(IPlatform platform)
        {
            if (_instance == null)
            {
                _instance = new App(platform);
            }
            return _instance;
        }

        private App(IPlatform platform)
        {
            PlatformInfo = platform;
            Cache = new LocalDataCache(this);

            InitializeComponent();
        }

        public async Task InitAsync()
        {
            bool setupMode = !(Properties.ContainsKey(_hasBeenSetupKey) && Properties[_hasBeenSetupKey].ToString() == true.ToString());
            await PlatformInfo.Authentication.Init(setupMode);
            if (!setupMode)
            {
                MainPage = new LoadingPage(Cache);
            }
            else
            {
                MainPage = new SetupPage(PlatformInfo);
            }
        }

        public async Task LoadAsync()
        {
            bool setupMode = !(Properties.ContainsKey(_hasBeenSetupKey) && Properties[_hasBeenSetupKey].ToString() == true.ToString());
            if (!setupMode)
            {
                await StartApp();
            }
        }

        internal async Task StartApp()
        {
            if (!(MainPage is LoadingPage))
            {
                await MainPage.Navigation.PushModalAsync(new LoadingPage(Cache));
            }
            await Cache.Init();
            Properties[_hasBeenSetupKey] = true.ToString();
            await SavePropertiesAsync();
            MainPage = new RootPage(this);

            PlatformInfo.Authentication.LoginEnabled += Authentication_LoginEnabled;
            PlatformInfo.Authentication.LoggingOff += Authentication_LoggingOff;
        }

        private async void Authentication_LoginEnabled(object sender, EventArgs e)
        {
            await Cache.UploadRecentItems();
        }

        private void Authentication_LoggingOff(object sender, EventArgs e)
        {
            Cache.Save();
        }

        internal async Task PopToRoot()
        {
            await (MainPage as RootPage).Detail.Navigation.PopToRootAsync();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Cache.Dispose();
        }
    }
}
