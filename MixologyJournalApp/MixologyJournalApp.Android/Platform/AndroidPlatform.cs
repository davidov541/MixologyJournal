using MixologyJournalApp.Platform;

namespace MixologyJournalApp.Droid.Platform
{
    public class AndroidPlatform : IPlatform
    {
        public IAlertDialogFactory AlertDialogFactory { get; }

        public IBackend Backend { get; }

        public AuthenticationManager Authentication { get; }

        internal AndroidPlatform(MainActivity context)
        {
            Authentication = new AuthenticationManager(new Auth0LoginMethod(context), new LocalLoginMethod());

            AlertDialogFactory = new AlertDialogFactory(context);

            Backend = new BackendManager(Authentication);
        }
    }
}