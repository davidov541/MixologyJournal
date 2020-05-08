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

        public async Task LoadAsync()
        {
            await PlatformInfo.Backend.Init();
            if (Properties.ContainsKey(_hasBeenSetupKey) && Properties[_hasBeenSetupKey].ToString() == true.ToString() && false)
            {
                MainPage = new LoadingPage(Cache);
                await StartApp();
            }
            else
            {
                MainPage = new SetupPage(PlatformInfo);
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
        }
    }
}
