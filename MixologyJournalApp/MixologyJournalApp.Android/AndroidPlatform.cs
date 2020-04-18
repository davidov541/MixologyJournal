using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MixologyJournalApp.Droid.Model;
using MixologyJournalApp.Droid.View;
using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using MixologyJournalApp.View;

namespace MixologyJournalApp.Droid
{
    public class AndroidPlatform : IPlatform
    {
        private AlertDialogFactory _factory;
        public IAlertDialogFactory AlertDialogFactory
        {
            get
            {
                return _factory;
            }
        }

        private BackendManager _backend;
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
            _backend = new BackendManager(context);
        }
    }
}