using System;
using Android.App;
using Android.Runtime;
using MixologyJournal.Droid.ViewModel;

namespace MixologyJournalApp.Droid
{
    [Application]
    public class MixologyApplication : Application
    {
        private MixologyApp _app;
        internal MixologyApp App
        {
            get
            {
                return _app;
            }
        }

        private Activity _currentActivity;
        public Activity CurrentActivity
        {
            get
            {
                return _currentActivity;
            }
        }

        public MixologyApplication() : base()
        {
            Init();
        }

        public MixologyApplication(IntPtr javaReference, JniHandleOwnership transfer) : 
            base(javaReference, transfer)
        {
            Init();
        }

        private void Init() 
        {
            _app = new MixologyApp(BaseContext, this);
            SetTheme(Resource.Style.AppTheme);
        }

        public void SetNewActivity(Activity activity)
        {
            _currentActivity = activity;
        }
    }
}
