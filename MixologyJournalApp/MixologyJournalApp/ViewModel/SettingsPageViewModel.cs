using MixologyJournalApp.Platform;
using System.Collections.Generic;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class SettingsPageViewModel
    {
        private readonly IPlatform _platform;
        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _platform.Backend.LoginMethods;
            }
        }

        public SettingsPageViewModel(App app)
        {
            _platform = app.PlatformInfo;
        }
   }
}
