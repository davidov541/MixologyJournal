using System;

namespace MixologyJournalApp.View
{
    public interface IAlertDialogFactory
    {
        void showDialog(String title, String message);
    }
}
