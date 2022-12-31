using System.ComponentModel;

namespace MixologyJournalApp.MAUI.ViewModel
{
    public class StepViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal int Index
        {
            get;
            private set;
        }

        private String _text;
        public String Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public StepViewModel(String text, int index)
        {
            Text = text;
            Index = index;
        }
    }
}
