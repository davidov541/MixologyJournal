using System.ComponentModel;

namespace MixologyJournal.ViewModel.List
{
    public interface IListableViewModel : IListEntry, INotifyPropertyChanged
    {
        int ID
        {
            get;
        }
    }
}
