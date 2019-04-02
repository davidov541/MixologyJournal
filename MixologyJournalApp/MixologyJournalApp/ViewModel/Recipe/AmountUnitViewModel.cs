using System;
using System.ComponentModel;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IAmountUnitViewModel
    {
        String UserVisibleName
        {
            get;
        }

        bool QuantityMatters
        {
            get;
        }
    }

    internal class AmountUnitViewModel : IAmountUnitViewModel, INotifyPropertyChanged
    {
        private BaseMixologyApp _app;
        private String _nameKey;
        private AmountUnit _sourceModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public String UserVisibleName
        {
            get
            {
                return _app.GetLocalizedString(_nameKey);
            }
        }

        internal String IngredientDescriptionFormat
        {
            get
            {
                return _app.GetLocalizedString(_nameKey + "Format");
            }
        }

        internal AmountUnit Model
        {
            get
            {
                return _sourceModel;
            }
        }

        internal String UnitInPlural
        {
            get
            {
                return _app.GetLocalizedString(_nameKey + "Plural");
            }
        }

        private bool _quantityMatters;
        public bool QuantityMatters
        {
            get
            {
                return _quantityMatters;
            }
        }

        internal AmountUnitViewModel(String nameKey, bool quantityMatters, AmountUnit model, BaseMixologyApp app)
        {
            _nameKey = nameKey;
            _sourceModel = model;
            _app = app;
            _quantityMatters = quantityMatters;
        }

        private void OnPropertyChanged(String property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public override Boolean Equals(Object obj)
        {
            AmountUnitViewModel other = obj as AmountUnitViewModel;
            if (other == null)
            {
                return false;
            }
            if (!other._nameKey.Equals(_nameKey))
            {
                return false;
            }
            if (other._sourceModel != _sourceModel)
            {
                return false;
            }
            return true;
        }

        public override Int32 GetHashCode()
        {
            return _nameKey.GetHashCode() * 17 + _sourceModel.GetHashCode();
        }
    }
}
