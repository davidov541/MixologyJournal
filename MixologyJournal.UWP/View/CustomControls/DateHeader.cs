using System;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.List;

namespace MixologyJournal.View
{
    public class DateHeader : IListEntryHeader
    {
        private DateTime _date;

        public String Caption
        {
            get
            {
                return String.Empty;
            }
        }

        public String Title
        {
            get
            {
                return _date.ToString("d");
            }
        }

        public Symbol? Icon
        {
            get
            {
                return null;
            }
        }

        public DateHeader(DateTime date)
        {
            _date = date;
        }
    }
}
