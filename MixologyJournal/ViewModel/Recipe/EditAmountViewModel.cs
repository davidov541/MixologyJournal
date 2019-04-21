using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using Newtonsoft.Json;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IEditAmountViewModel : INotifyPropertyChanged
    {
        IEnumerable<IAmountUnitViewModel> AvailableUnits
        {
            get;
        }

        IAmountUnitViewModel Unit
        {
            get;
            set;
        }

        double Quantity
        {
            get;
            set;
        }

        int ServingsNumber
        {
            get;
            set;
        }

        void Save();

        void Cancel();

        void Reset();
    }

    internal class EditAmountViewModel : AmountViewModel, IEditAmountViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public int ServingsNumber
        {
            get
            {
                return _servingsNumber;
            }
            set
            {
                _servingsNumber = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(ServingsNumber));
            }
        }

        public IAmountUnitViewModel Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value as AmountUnitViewModel;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Unit));
            }
        }

        [JsonIgnore]
        public IEnumerable<IAmountUnitViewModel> AvailableUnits
        {
            get
            {
                return GetAvailableUnits();
            }
        }

        public EditAmountViewModel(Amount amount, BaseMixologyApp app) :
            base(amount, app)
        {
            Reset();
        }

        public EditAmountViewModel(AmountViewModel viewModel) :
           base(viewModel)
        {
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override bool Equals(object obj)
        {
            EditAmountViewModel other = obj as EditAmountViewModel;
            return other != null && 
                (Description.Equals(other.Description)) &&
                (Quantity.Equals(other.Quantity)) &&
                (ServingsNumber.Equals(other.ServingsNumber)) &&
                (Unit.Equals(other.Unit));
        }

        public void Save()
        {
            _model.Quantity = Quantity;
            _model.Unit = _unit.Model;
        }

        public void Cancel()
        {
            Reset();
        }

        public void Reset()
        {
            Unit = GetAvailableUnits().FirstOrDefault(u => u.Model == _model.Unit);
            Quantity = _model.Quantity;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
