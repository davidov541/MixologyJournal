using MixologyJournalApp.Model;
using System;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel: INotifyPropertyChanged
    {
        private Recipe _model;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public RecipeViewModel(Recipe model)
        {
            _model = model;
        }
    }
}
