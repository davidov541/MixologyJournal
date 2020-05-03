using System;
using System.Collections.Generic;
using System.Text;

namespace MixologyJournalApp.View
{
    public interface IMasterMenuItem
    {
        String Title
        {
            get;
        }

        Type TargetType
        {
            get;
        }
    }
}
