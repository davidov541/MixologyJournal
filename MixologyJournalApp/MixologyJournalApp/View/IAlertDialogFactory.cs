using System;
using System.Collections.Generic;
using System.Text;

namespace MixologyJournalApp.View
{
    public interface IAlertDialogFactory
    {
        void showDialog(String title, String message);
    }
}
