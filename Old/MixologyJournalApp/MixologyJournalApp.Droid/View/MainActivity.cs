using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Widget;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournalApp.Droid.View
{
    [Activity(Theme = "@style/AppTheme")]
    public class MainActivity : FragmentActivity
    {
        private MixologyApplication _app;
        private IOverviewPageViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainPage);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetActionBar(toolbar);

            ActionBar.Title = "Mixology Journal";
            ActionBar.SetDisplayShowCustomEnabled(false);

            _app = Application as MixologyApplication;
            _app.SetNewActivity(this);
            _viewModel = _app.App.GetOverviewPage();
            _viewModel.MinimumEntriesPerGrouping = 0;

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Clickable = true;
            fab.Click += CreateNewRecipe;

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            HomePageAdapter adapter = new HomePageAdapter(SupportFragmentManager, _app, _viewModel);
            viewPager.Adapter = adapter;
            viewPager.AddOnPageChangeListener(new FABOnPageChangeListener(fab));
        }

        private void CreateNewRecipe(object sender, EventArgs e)
        {
            _viewModel.CreateBaseRecipeAsync().ContinueWith(r =>
            {
                IBaseRecipePageViewModel newRecipe = r.Result;
                Intent intent = new Intent(_app.ApplicationContext, typeof(AddRecipeActivity));
                intent.PutExtra("RecipeID", newRecipe.ID);
                _app.StartActivity(intent);
            });
        }
    }
}


