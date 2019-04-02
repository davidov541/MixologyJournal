using Android.Support.Design.Widget;
using static Android.Support.Design.Widget.FloatingActionButton;
using static Android.Support.V4.View.ViewPager;

namespace MixologyJournalApp.Droid.View
{
    public class FABOnPageChangeListener : SimpleOnPageChangeListener
    {
        private FloatingActionButton _fab;

        public FABOnPageChangeListener(FloatingActionButton fab)
        {
            _fab = fab;   
        }

        public override void OnPageSelected(int position)
        {
            _fab.Hide(new DelayedVisibilityListener());
        }

        private class DelayedVisibilityListener : OnVisibilityChangedListener
        {
            public override void OnHidden(FloatingActionButton fab)
            {
                base.OnHidden(fab);
                fab.Show();
            }
        }
    }
}
