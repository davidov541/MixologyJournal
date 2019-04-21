using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Droid.View
{
    [Activity(Theme = "@style/AppTheme")]
    public class AddRecipeActivity : Activity
    {
        private MixologyApplication _app;
        private IBaseRecipeEditPageViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddRecipe);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetActionBar(toolbar);

            ActionBar.Title = "Add Recipe";
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayUseLogoEnabled(false);
            ActionBar.SetDisplayOptions(0, ActionBarDisplayOptions.UseLogo);
            ActionBar.SetDisplayShowCustomEnabled(true);

            ImageButton button = FindViewById<ImageButton>(Resource.Id.saveButton);
            button.Visibility = ViewStates.Visible;
            button.Click += SaveRecipe;

            int id = Intent.GetIntExtra("RecipeID", -1);

            _app = Application as MixologyApplication;
            _viewModel = _app.App.GetBaseRecipeEditPage(id);
            _app.SetNewActivity(this);

            ListView listView = (ListView)FindViewById(Resource.Id.ingredients);
            _viewModel.AddIngredient();
            listView.Adapter = new IngredientEntryListItemAdapter(this, _viewModel.Recipe);
        }

        public override bool OnNavigateUp()
        {
            _viewModel.DeleteAsync().ContinueWith(r =>
            {
                Finish();
            });
            return base.OnNavigateUp();
        }

        private void SaveRecipe(object sender, EventArgs e)
        {
            TextInputEditText titleText = FindViewById<TextInputEditText>(Resource.Id.name);
            _viewModel.Recipe.SetTitle(titleText.Text);
            TextInputEditText instructionsText = FindViewById<TextInputEditText>(Resource.Id.instructions);
            _viewModel.Recipe.SetInstructions(instructionsText.Text);

            _viewModel.SaveAsync().ContinueWith(r =>
            {
                Finish();

                Intent intent = new Intent(_app.ApplicationContext, typeof(MainActivity));
                _app.StartActivity(intent);
            });
        }
    }
}
