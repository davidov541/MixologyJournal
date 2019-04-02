using System;
using System.ComponentModel;
using System.Linq;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IViewAmountViewModel : INotifyPropertyChanged
    {
        IAmountUnitViewModel Unit
        {
            get;
        }

        double Quantity
        {
            get;
        }

        int ServingsNumber
        {
            get;
            set;
        }
    }

    internal class ViewAmountViewModel : AmountViewModel, IViewAmountViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double Quantity
        {
            get
            {
                return _quantity;
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
                OnPropertyChanged(nameof(ServingsNumber));
            }
        }

        public IAmountUnitViewModel Unit
        {
            get
            {
                return _unit;
            }
        }

        public ViewAmountViewModel(Amount amount, BaseMixologyApp app) :
            base(amount, app)
        {
            _unit = GetAvailableUnits().FirstOrDefault(u => u.Model == _model.Unit);
            _quantity = _model.Quantity;
        }

        public ViewAmountViewModel(AmountViewModel viewModel) :
           base(viewModel)
        {
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
