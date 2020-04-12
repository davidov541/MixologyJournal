using Android.App;
using Android.Content;
using MixologyJournalApp.View;
using System;

namespace MixologyJournalApp.Droid.View
{
    internal class AlertDialogFactory: IAlertDialogFactory
    {
        private Context _context;

        public AlertDialogFactory(Context context)
        {
            _context = context;
        }

        public void showDialog(String title, String message)
        {
            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(_context);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}