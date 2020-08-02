namespace MixologyJournalApp.Platform
{
    public interface IPlatform
    {
        IAlertDialogFactory AlertDialogFactory
        {
            get;
        }

        BackendManager Backend
        {
            get;
        }

        AuthenticationManager Authentication
        {
            get;
        }
    }
}
