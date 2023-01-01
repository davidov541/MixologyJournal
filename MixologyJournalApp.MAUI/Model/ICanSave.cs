using MixologyJournalApp.MAUI.Data;

namespace MixologyJournalApp.MAUI.Model
{
    internal interface ICanSave
    {
        Task SaveAsync(IStateSaver stateSaver);
    }
}
