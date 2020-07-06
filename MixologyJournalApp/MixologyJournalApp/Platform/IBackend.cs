using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public interface IBackend : INotifyPropertyChanged
    {
        Boolean IsAuthenticated
        {
            get;
        }

        IEnumerable<ILoginMethod> LoginMethods
        {
            get;
        }

        User User
        {
            get;
        }

        Task Init(bool setupMode);

        Task<QueryResult> PostResult(String path, Object body);

        Task<QueryResult> DeleteResult(String path, Object body);

        Task<String> GetResult(String path);
    }
}
