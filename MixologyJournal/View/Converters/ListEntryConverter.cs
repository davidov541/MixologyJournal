using System;
using System.Collections.Generic;
using System.Linq;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.List;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public abstract class ListEntryConverter : IValueConverter
    {
        public abstract Object Convert(Object value, Type targetType, Object parameter, String language);

        public IEnumerable<IListEntry> FilterList<T>(IEnumerable<IListEntry> entries) where T : IJournalEntryViewModel
        {
            List<IListEntry> result = new List<IListEntry>();
            foreach (IGrouping<DateTime, T> grouping in entries.OfType<T>().GroupBy(e => e.CreationDate.Date).OrderBy(g => g.Key))
            {
                result.Add(new DateHeader(grouping.Key));
                foreach (T entry in grouping)
                {
                    result.Add(entry);
                }
            }
            return result;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
