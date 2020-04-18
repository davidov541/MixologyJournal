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

namespace MixologyJournalApp.Droid.Platform
{
    internal class AndroidPlatform
    {
        private static AndroidPlatform _instance;

        public Context CurrentContext
        {
            get;
            set;
        }

        public static void Init(Context context)
        {
            _instance = new AndroidPlatform(context);
        }

        public static AndroidPlatform getInstance()
        {
            return _instance;
        }

        private AndroidPlatform(Context context)
        {
            CurrentContext = context;
        }
    }
}