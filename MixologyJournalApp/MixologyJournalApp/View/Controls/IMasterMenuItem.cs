using System;

namespace MixologyJournalApp.View.Controls
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
