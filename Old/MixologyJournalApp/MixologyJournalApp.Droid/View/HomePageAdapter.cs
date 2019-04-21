using System;
using Android.Support.V4.App;
using Java.Lang;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournalApp.Droid.View
{
    public class HomePageAdapter : FragmentPagerAdapter
    {
        private Fragment[] _fragments = new Fragment[2];

        internal HomePageAdapter(FragmentManager fm, MixologyApplication app, IOverviewPageViewModel viewModel) : base(fm)
        {
            _fragments[0] = new RecipeListFragment(viewModel, app);
            _fragments[1] = new DrinkListFragment(viewModel, app);
        }

        public override int Count {
            get
            {
                return 2;
            }
        }

        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            switch(position)
            {
                case 0:
                    return new Java.Lang.String("Recipes");
                case 1:
                    return new Java.Lang.String("Drinks");
                default:
                    throw new NotSupportedException("A non-supported index!");
            }
        }
    }
}
