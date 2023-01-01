using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixologyJournalApp.MAUI.Data
{
    internal interface IStateSaver
    {
        Task InsertOrReplaceAsync<T>(T state) where T : new();
    }
}
