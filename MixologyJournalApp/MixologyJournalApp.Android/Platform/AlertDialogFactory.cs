﻿using Android.App;
using Android.Content;
using MixologyJournalApp.Platform;
using System;

namespace MixologyJournalApp.Droid.Platform
{
    internal class AlertDialogFactory: IAlertDialogFactory
    {
        private Context _context;

        public AlertDialogFactory(Context context)
        {
            _context = context;
        }

        public void ShowDialog(String title, String message)
        {
            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(_context);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}