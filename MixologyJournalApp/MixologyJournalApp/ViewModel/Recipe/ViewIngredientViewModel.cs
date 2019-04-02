using System;
using System.ComponentModel;
using System.Globalization;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IIngredientViewModel : INotifyPropertyChanged
    {
        String Description
        {
            get;
        }

        int ServingsNumber
        {
            get;
        }
    }

    internal class ViewIngredientViewModel : IngredientViewModel, IIngredientViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ViewAmountViewModel _amount;

        public int ServingsNumber
        {
            get
            {
                return _amount.ServingsNumber;
            }
            set
            {
                _amount.ServingsNumber = value;
                OnPropertyChanged(nameof(ServingsNumber));
            }
        }

        public String Description
        {
            get
            {
                if (String.IsNullOrEmpty(_amount.Description) || String.IsNullOrEmpty(_name))
                {
                    return null;
                }
                bool isSingular = Math.Abs(_amount.Quantity - 1.0) < Double.Epsilon;
                String amountUnit = isSingular ? _amount.Unit.UserVisibleName : (_amount.Unit as AmountUnitViewModel).UnitInPlural;
                return String.Format(CultureInfo.CurrentCulture,
                                     (_amount.Unit as AmountUnitViewModel).IngredientDescriptionFormat,
                                     _amount.Description,
                                     amountUnit,
                                     _name);
            }
        }

        public ViewIngredientViewModel(BaseMixologyApp app) :
            this(new Ingredient(), app)
        {
        }

        public ViewIngredientViewModel(Ingredient model, BaseMixologyApp app) :
            base(model, app)
        {
            _amount = new ViewAmountViewModel(model.Amount, app);
        }

        public ViewIngredientViewModel(IngredientViewModel viewModel) :
            base(viewModel)
        {
            _amount = new ViewAmountViewModel(viewModel.InternalAmount);
            InternalAmount = _amount;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
