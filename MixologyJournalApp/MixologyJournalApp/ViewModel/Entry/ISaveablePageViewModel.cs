using System.Threading.Tasks;
using MixologyJournal.ViewModel.List;

namespace MixologyJournal.ViewModel.Entry
{
    public interface ISaveablePageViewModel : IListableViewModel, IPageViewModel
    {
        Task SaveAsync();

        void Cancel();

        Task DeleteAsync();
    }
}
