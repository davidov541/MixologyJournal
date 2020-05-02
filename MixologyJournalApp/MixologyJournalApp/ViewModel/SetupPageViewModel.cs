using MixologyJournalApp.Platform;
using System.Collections.Generic;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageViewModel
    {
        private IPlatform _platform;
        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _platform.Backend.LoginMethods;
            }
        }

        public SetupPageViewModel(IPlatform platform)
        {
            _platform = platform;
        }
    }
}
