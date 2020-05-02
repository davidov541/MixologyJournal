using Android.Content;
using MixologyJournalApp.Platform;

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

        internal AndroidPlatform(Context context)
        {
            _factory = new AlertDialogFactory(context);
            _backend = new BackendManager();
        }
    }
}