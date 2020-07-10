using MixologyJournalApp.Platform;
using System.ComponentModel;

namespace MixologyJournalApp.Droid.Platform
{
    public class AndroidPlatform : IPlatform
    {
        private readonly AlertDialogFactory _factory;
        public IAlertDialogFactory AlertDialogFactory
        {
            get
            {
                return _factory;
            }
        }

        private readonly BackendManager _backend;
        public IBackend Backend
        {
            get
            {
                return _backend;
            }
        }

        public AuthenticationManager Authentication { get; }

        internal AndroidPlatform(MainActivity context)
        {
            Authentication = new AuthenticationManager(new Auth0LoginMethod(context));

            _factory = new AlertDialogFactory(context);

            _backend = new BackendManager(Authentication);
        }
    }
}