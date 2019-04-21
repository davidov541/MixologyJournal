using System;
using System.ComponentModel;
using System.Globalization;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IEditIngredientViewModel : INotifyPropertyChanged
    {
        String Name
        {
            get;
            set;
        }

        String Details
        {
            get;
            set;
        }

        IEditAmountViewModel Amount
        {
            get;
        }

        String Description
        {
            get;
        }
    }

    internal class EditIngredientViewModel : IngredientViewModel, IEditIngredientViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
            }
        }

        public String Details
        {
            get
            {
                return _details;
            }
            set
            {
                _details = value;
                OnPropertyChanged(nameof(Details));
            }
        }

        private EditAmountViewModel _amount;
        public IEditAmountViewModel Amount
        {
            get
            {
                return _amount;
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

        public EditIngredientViewModel(BaseMixologyApp app) :
            this(new Ingredient(), app)
        {
        }

        public EditIngredientViewModel(Ingredient model, BaseMixologyApp app) :
            base(model, app)
        {
            _amount = new EditAmountViewModel(model.Amount, app);
            InternalAmount = _amount;
            _amount.PropertyChanged += Amount_PropertyChanged;
        }

        public EditIngredientViewModel(IngredientViewModel viewModel) :
            base(viewModel)
        {
            _amount = new EditAmountViewModel(viewModel.InternalAmount);
            InternalAmount = _amount;
            _amount.PropertyChanged += Amount_PropertyChanged;
        }

        private void Amount_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EditAmountViewModel.Unit):
                case nameof(AmountViewModel.Description):
                    OnPropertyChanged(nameof(Description));
                    break;
                default:
                    break;
            }
        }

        public void Save()
        {
            _model.Name = Name;
            _model.Details = Details;
            Amount.Save();
        }

        public void Cancel()
        {
            Name = _model.Name;
            Details = _model.Details;
            Amount.Cancel();
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
