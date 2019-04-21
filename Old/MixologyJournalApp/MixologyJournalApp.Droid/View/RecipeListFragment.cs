using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MixologyJournal.ViewModel.Entry;
using AndroidView = Android.Views.View;
using Fragment = Android.Support.V4.App.Fragment;

namespace MixologyJournalApp.Droid.View
{
    public class RecipeListFragment : Fragment
    {
        private MixologyApplication _app;
        private IOverviewPageViewModel _viewModel;

        internal RecipeListFragment(IOverviewPageViewModel viewModel, MixologyApplication app) : base()
        {
            _viewModel = viewModel;
            _app = app;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override AndroidView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            AndroidView view = inflater.Inflate(Resource.Layout.RecipeList, container, false);
            ListView listView = (ListView)view.FindViewById(Resource.Id.recipeList);
            listView.ItemClick += ListView_ItemClick;

            listView.Adapter = new SingleLineListItemAdapter(Activity, _viewModel.Recipes.ToList());
            return view;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(_app.ApplicationContext, typeof(RecipeActivity));
            intent.PutExtra("RecipeID", (int)e.Id);
            _app.StartActivity(intent);
        }
    }
}
