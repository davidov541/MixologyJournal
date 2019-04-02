using System;
using System.ComponentModel;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.ViewModel.Recipe
{
    enum AmountDiffStatus
    {
        Increased,
        Decreased,
        Unchanged,
        // Used if units changed to or from a unit which we can't convert between.
        ChangedButUnknown
    }

    internal class AmountDiff : INotifyPropertyChanged
    {
        private ViewAmountViewModel _baseAmount;
        private EditAmountViewModel _modifiedAmount;

        public event PropertyChangedEventHandler PropertyChanged;

        public AmountDiffStatus Status
        {
            get
            {
                double baseQuantity = GetAmountInOunces((_baseAmount.Unit as AmountUnitViewModel).Model, _baseAmount.Quantity);
                double modifiedQuantity = GetAmountInOunces((_modifiedAmount.Unit as AmountUnitViewModel).Model, _modifiedAmount.Quantity);
                if (Double.IsNaN(baseQuantity) || Double.IsNaN(modifiedQuantity))
                {
                    if (_baseAmount.Unit.Equals(_modifiedAmount.Unit))
                    {
                        if (_baseAmount.Quantity > _modifiedAmount.Quantity)
                        {
                            return AmountDiffStatus.Decreased;
                        }
                        else if (_baseAmount.Quantity < _modifiedAmount.Quantity)
                        {
                            return AmountDiffStatus.Increased;
                        }
                        else
                        {
                            return AmountDiffStatus.Unchanged;
                        }
                    }
                    else
                    {
                        return AmountDiffStatus.ChangedButUnknown;
                    }
                }
                if (baseQuantity < modifiedQuantity)
                {
                    return AmountDiffStatus.Increased;
                }
                else if (baseQuantity > modifiedQuantity)
                {
                    return AmountDiffStatus.Decreased;
                }
                else
                {
                    return AmountDiffStatus.Unchanged;
                }
            }
        }

        public double Quantity
        {
            get
            {
                return _modifiedAmount.Quantity;
            }
            set
            {
                _modifiedAmount.Quantity = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public IAmountUnitViewModel Unit
        {
            get
            {
                return _modifiedAmount.Unit;
            }
            set
            {
                _modifiedAmount.Unit = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public AmountDiff(ViewAmountViewModel baseAmount, EditAmountViewModel modifiedAmount)
        {
            _baseAmount = baseAmount;
            _modifiedAmount = modifiedAmount;
        }

        private static double GetAmountInOunces(AmountUnit unit, Double quantity)
        {
            switch (unit)
            {
                case AmountUnit.Centiliter:
                    // Liter = 33.814 ounces
                    return quantity * 0.33814;
                case AmountUnit.Cup:
                    // Cup = 8 ounces
                    return quantity * 8;
                case AmountUnit.Liter:
                    // Liter = 33.814 ounces
                    return quantity * 33.814;
                case AmountUnit.Milliliter:
                    // Liter = 33.814 ounces
                    return quantity * 0.033814;
                case AmountUnit.Pint:
                    // Pint = 16 ounces
                    return quantity * 16.0;
                case AmountUnit.Tablespoon:
                    // Ounce = 2 Tablespoons
                    return quantity * 2.0;
                case AmountUnit.Dash:
                    // Ounce = 6 Teaspoons = 8 * 6 Dashes
                    return quantity / 8.0 / 6.0;
                case AmountUnit.Ounce:
                    return quantity;
                case AmountUnit.Teaspoon:
                case AmountUnit.Spoon:
                    // Ounce = 6 Teaspoons = 6 Bar Spoons
                    return quantity / 6.0;
                case AmountUnit.Drop:
                case AmountUnit.Pinch:
                case AmountUnit.Piece:
                case AmountUnit.Slice:
                case AmountUnit.Twist:
                    // Converting these to ounces makes no sense, so return NaN.
                    return Double.NaN;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
