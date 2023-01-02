using System.Linq.Expressions;

namespace MixologyJournalApp.MAUI.Data
{
    internal interface IStateSaver
    {
        Task InsertOrReplaceAsync<T>(T state) where T : new();

        Task<List<T>> GetFilteredItemsAsync<T>(Expression<Func<T, bool>> condition) where T : new();
    }
}
