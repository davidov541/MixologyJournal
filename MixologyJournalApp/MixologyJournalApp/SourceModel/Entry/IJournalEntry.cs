using System;
using System.Collections.Generic;

namespace MixologyJournal.SourceModel.Entry
{
    internal interface IJournalEntry
    {
        String Title
        {
            get;
        }

        String Notes
        {
            get;
            set;
        }

        DateTime CreationDate
        {
            get;
        }

        int ID
        {
            get;
        }

        IEnumerable<String> Pictures
        {
            get;
        }
    }
}
