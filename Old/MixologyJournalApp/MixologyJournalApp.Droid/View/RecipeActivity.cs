using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournalApp.Droid.View
{
    [Activity(Theme = "@style/AppTheme")]
    public class RecipeActivity : FragmentActivity
    {
        private MixologyApplication _app;
        private IBaseRecipePageViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RecipePage);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetActionBar(toolbar);

            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayUseLogoEnabled(false);
            ActionBar.SetDisplayOptions(0, ActionBarDisplayOptions.UseLogo);

            _app = Application as MixologyApplication;
            _app.SetNewActivity(this);

            int id = Intent.GetIntExtra("RecipeID", -1);
            _viewModel = _app.App.GetBaseRecipePage(id);

            ActionBar.Title = _viewModel.Title;

            ImageButton button = FindViewById<ImageButton>(Resource.Id.deleteButton);
            button.Visibility = ViewStates.Visible;
            button.Click += deleteButtonClick;;

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            RecipePageAdapter adapter = new RecipePageAdapter(SupportFragmentManager, _viewModel);
            viewPager.Adapter = adapter;
        }

        public override bool OnNavigateUp()
        {
            Finish();
            return base.OnNavigateUp();
        }

        private void deleteButtonClick(object sender, EventArgs e)
        {
            _viewModel.DeleteAsync().ContinueWith(t =>
            {
                Intent intent = new Intent(Application.Context, typeof(MainActivity));
                StartActivity(intent);
            });
        }
    }
}
