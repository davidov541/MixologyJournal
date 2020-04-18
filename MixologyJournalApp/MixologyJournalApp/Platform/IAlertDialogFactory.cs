using System;

namespace MixologyJournalApp.Platform
{
    public interface IAlertDialogFactory
    {
        void showDialog(String title, String message);
    }
}
