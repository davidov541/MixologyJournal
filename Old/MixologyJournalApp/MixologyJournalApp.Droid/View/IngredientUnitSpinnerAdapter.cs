using System;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Droid.View
{
    public class IngredientUnitSpinnerAdapter : BaseAdapter<IAmountUnitViewModel>
    {
        private IEditAmountViewModel _viewModel;
        private Context _context;

        public IngredientUnitSpinnerAdapter(Context context, IEditAmountViewModel viewModel)
        {
            _viewModel = viewModel;
            _context = context;
        }

        public override IAmountUnitViewModel this[int position] 
        {
            get
            {
                return _viewModel.AvailableUnits.ElementAt(position);
            }
        }

        public override int Count
        {
            get
            {
                return _viewModel.AvailableUnits.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            TextView text = new TextView(_context);
            text.SetText(_viewModel.AvailableUnits.ElementAt(position).UserVisibleName, TextView.BufferType.Normal);
            return text;
        }
    }
}
