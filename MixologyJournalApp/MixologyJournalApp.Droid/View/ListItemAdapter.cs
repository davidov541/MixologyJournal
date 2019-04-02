using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using MixologyJournal.ViewModel.List;

namespace MixologyJournalApp.Droid.View
{
    public class SingleLineListItemAdapter : BaseAdapter<IListEntry>
    {
        List<IListableViewModel> _items;
        Activity _context;
        Dictionary<int, IListEntryHeader> _headerIndicies;

        public SingleLineListItemAdapter(Activity context, IEnumerable<IListEntry> items) : base()
        {
            _context = context;
            _items = items.OfType<IListableViewModel>().ToList();
            int currIndex = 0;
            _headerIndicies = new Dictionary<int, IListEntryHeader>();
            foreach (IListEntry entry in items)
            {
                if (entry is IListEntryHeader)
                {
                    _headerIndicies[currIndex] = entry as IListEntryHeader;
                }
                else
                {
                    currIndex++;
                }
            }
        }

        public override long GetItemId(int position)
        {
            return _items[position].ID;
        }

        public override IListEntry this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            IListEntry item = _items[position];
            IListEntry prevItem = null;
            if (position > 0)
            {
                prevItem = _items[position - 1];
            }

            Android.Views.View view = convertView;
            if (view == null) // no view to re-use, create new
                view = _context.LayoutInflater.Inflate(Resource.Layout.SingleLineListItem, null);
            view.FindViewById<TextView>(Resource.Id.textView1).Text = item.Title;
            String header = String.Empty;
            if (_headerIndicies.ContainsKey(position))
            {
                header = _headerIndicies[position].Title;
            }
            view.FindViewById<TextView>(Resource.Id.letterHeader).Text = header;
            return view;
        }
    }
}
