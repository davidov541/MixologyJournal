using MixologyJournalApp.Model;
using MixologyJournalApp.View;

namespace MixologyJournalApp.Platform
{
    public interface IPlatform
    {
        IAlertDialogFactory AlertDialogFactory
        {
            get;
        }

        IBackend Backend
        {
            get;
        }
    }
}
