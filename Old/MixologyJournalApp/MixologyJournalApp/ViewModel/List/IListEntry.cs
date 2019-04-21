using System;

namespace MixologyJournal.ViewModel.List
{
    public interface IListEntry
    {
        String Title
        {
            get;
        }

        String Caption
        {
            get;
        }

        Symbol? Icon
        {
            get;
        }
    }
}
