using System;
using System.Linq;
using Android.App;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Droid.View
{
    public class IngredientEntryListItemAdapter : BaseAdapter<IEditIngredientViewModel>
    {
        private IBaseEditRecipeViewModel _viewModel;
        private Activity _context;

        public IngredientEntryListItemAdapter(Activity context, IBaseEditRecipeViewModel viewModel) : base()
        {
            _context = context;
            _viewModel = viewModel;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override IEditIngredientViewModel this[int position]
        {
            get { return _viewModel.Ingredients.ElementAt(position); }
        }

        public override int Count
        {
            get { return _viewModel.Ingredients.Count(); }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Android.Views.View view = convertView;
            bool newlyCreated = view == null;
            if (newlyCreated)
            {
                IEditIngredientViewModel ingredient = _viewModel.Ingredients.ElementAt(position);
                view = _context.LayoutInflater.Inflate(Resource.Layout.IngredientEntryListItem, null);

                TextInputEditText ingredientName = view.FindViewById<TextInputEditText>(Resource.Id.name);
                // ingredientName.TextChanged += (sender, e) => IngredientName_TextChanged(ingredientName, e, position);

                TextInputEditText ingredientAmount = view.FindViewById<TextInputEditText>(Resource.Id.amount);
                // ingredientAmount.TextChanged += (sender, e) => IngredientName_TextChanged(ingredientAmount, e, position);

                Spinner ingredientUnit = view.FindViewById<Spinner>(Resource.Id.unit);
                // ingredientUnit.ItemSelected += (sender, e) => ;
                IngredientUnitSpinnerAdapter adapter = new IngredientUnitSpinnerAdapter(_context, ingredient.Amount);
                ingredientUnit.Adapter = adapter;
                int selectedIndex = 0;
                foreach (IAmountUnitViewModel unit in ingredient.Amount.AvailableUnits)
                {
                    if (unit.Equals(ingredient.Amount.Unit))
                    {
                        break;
                    }
                    selectedIndex++;
                }
                ingredientUnit.SetSelection(selectedIndex);
            }
            return view;
        }

        private void IngredientName_TextChanged(TextInputEditText sender, TextChangedEventArgs e, int position)
        {
            /*
            if (e.BeforeCount == 0 && e.AfterCount == 1)
            {
                _viewModel.AddIngredient();
                NotifyDataSetChanged();
            }
            else if (e.BeforeCount == 1 && e.AfterCount == 0)
            {
                _viewModel.RemoveIngredient(_viewModel.Ingredients.Last());
                NotifyDataSetChanged();
            }
            _viewModel.Ingredients.ElementAt(position).Name = sender.Text;
            */
        }
    }
}
