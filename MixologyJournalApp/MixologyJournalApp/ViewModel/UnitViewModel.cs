using MixologyJournalApp.Model;
using System;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class UnitViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Unit _model;

        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public UnitViewModel(Unit model)
        {
            _model = model;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
