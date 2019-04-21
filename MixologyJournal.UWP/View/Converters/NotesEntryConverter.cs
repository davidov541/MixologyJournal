using System;
using System.Collections.Generic;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.List;

namespace MixologyJournal.View
{
    public class NotesEntryConverter : ListEntryConverter
    {
        public override Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            return FilterList<INotesEntryViewModel>(value as IEnumerable<IListEntry>);
        }
    }
}
