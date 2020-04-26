using System;

namespace MixologyJournalApp.Platform
{
    public interface IAlertDialogFactory
    {
        void ShowDialog(String title, String message);
    }
}
