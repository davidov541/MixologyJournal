using System;
using System.ComponentModel;

namespace MixologyJournal.ViewModel.Recipe
{
    enum ChangedStatus
    {
        Added,
        Removed,
        Modified,
        Replaced,
        Unchanged
    }

    internal class IngredientDiff : INotifyPropertyChanged
    {
        private EditIngredientViewModel _baseIngredient;
        private EditIngredientViewModel _modifiedIngredient;

        public event PropertyChangedEventHandler PropertyChanged;

        public ChangedStatus Status
        {
            get
            {
                if (_baseIngredient == null)
                {
                    return ChangedStatus.Added;
                }
                else if (_modifiedIngredient == null)
                {
                    return ChangedStatus.Removed;
                }
                else if (!_baseIngredient.Name.Equals(_modifiedIngredient.Name))
                {
                    return ChangedStatus.Replaced;
                }
                else if (Amount.Status != AmountDiffStatus.Unchanged)
                {
                    return ChangedStatus.Modified;
                }
                return ChangedStatus.Unchanged;
            }
        }

        public String OldName
        {
            get
            {
                if (_baseIngredient != null)
                {
                    return _baseIngredient.Name;
                }
                return String.Empty;
            }
        }

        public String Name
        {
            get
            {
                if (_modifiedIngredient != null)
                {
                    return _modifiedIngredient.Name;
                }
                return _baseIngredient.Name;
            }
            /*
             * TODO: Why did we need a set here?
            set
            {
                if (_modifiedIngredient != null)
                {
                    _modifiedIngredient.Name = value;
                }
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Status));
            }
            */
        }

        public String Details
        {
            get
            {
                if (_modifiedIngredient != null)
                {
                    return _modifiedIngredient.Details;
                }
                return _baseIngredient.Details;
            }
            /*
             * TODO: Why did we need a set here?
            set
            {
                if (_modifiedIngredient != null)
                {
                    _modifiedIngredient.Details = value;
                }
                OnPropertyChanged(nameof(Details));
            }
            */
        }

        public AmountDiff Amount
        {
            get;
            private set;
        }

        public IngredientDiff(EditIngredientViewModel baseIngredient, EditIngredientViewModel modifiedIngredient)
        {
            _baseIngredient = baseIngredient;
            _modifiedIngredient = modifiedIngredient;
            ViewAmountViewModel initialAmount = baseIngredient == null ? null : new ViewAmountViewModel(baseIngredient?.Amount as AmountViewModel);
            EditAmountViewModel newAmount = modifiedIngredient == null ? null : modifiedIngredient?.Amount as EditAmountViewModel;
            Amount = new AmountDiff(initialAmount, newAmount);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
