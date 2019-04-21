using System;
using System.Collections.Generic;
using System.Globalization;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using Newtonsoft.Json;

namespace MixologyJournal.ViewModel.Recipe
{
    internal class AmountViewModel
    {
        protected Amount _model;
        protected BaseMixologyApp _app;
        protected AmountUnitViewModel _unit;
        protected double _quantity;
        protected int _servingsNumber = 1;

        public String Description
        {
            get
            {
                return String.Format(CultureInfo.CurrentCulture, "{0}", _quantity * _servingsNumber);
            }
        }

        public AmountViewModel(Amount amount, BaseMixologyApp app)
        {
            _model = amount;
            _app = app;
        }

        public AmountViewModel(AmountViewModel viewModel) :
            this(viewModel._model, viewModel._app)
        {
            _unit = viewModel._unit;
            _quantity = viewModel._quantity;
            _servingsNumber = viewModel._servingsNumber;
        }

        private List<AmountUnitViewModel> _availableUnits;
        protected IEnumerable<AmountUnitViewModel> GetAvailableUnits()
        {
            if (_availableUnits == null)
            {
                _availableUnits = new List<AmountUnitViewModel>();
                _availableUnits.Add(new AmountUnitViewModel("OunceUnit", true, AmountUnit.Ounce, _app));
                _availableUnits.Add(new AmountUnitViewModel("MilliliterUnit", true, AmountUnit.Milliliter, _app));
                _availableUnits.Add(new AmountUnitViewModel("CentiliterUnit", true, AmountUnit.Centiliter, _app));
                _availableUnits.Add(new AmountUnitViewModel("LiterUnit", true, AmountUnit.Liter, _app));
                _availableUnits.Add(new AmountUnitViewModel("PintUnit", true, AmountUnit.Pint, _app));
                _availableUnits.Add(new AmountUnitViewModel("SpoonUnit", true, AmountUnit.Spoon, _app));
                _availableUnits.Add(new AmountUnitViewModel("DashUnit", true, AmountUnit.Dash, _app));
                _availableUnits.Add(new AmountUnitViewModel("TeaspoonUnit", true, AmountUnit.Teaspoon, _app));
                _availableUnits.Add(new AmountUnitViewModel("TablespoonUnit", true, AmountUnit.Tablespoon, _app));
                _availableUnits.Add(new AmountUnitViewModel("CupUnit", true, AmountUnit.Cup, _app));
                _availableUnits.Add(new AmountUnitViewModel("DropUnit", true, AmountUnit.Drop, _app));
                _availableUnits.Add(new AmountUnitViewModel("PinchUnit", true, AmountUnit.Pinch, _app));
                _availableUnits.Add(new AmountUnitViewModel("SliceUnit", true, AmountUnit.Slice, _app));
                _availableUnits.Add(new AmountUnitViewModel("TwistUnit", true, AmountUnit.Twist, _app));
                _availableUnits.Add(new AmountUnitViewModel("PieceUnit", true, AmountUnit.Piece, _app));
                _availableUnits.Add(new AmountUnitViewModel("UnknownUnit", false, AmountUnit.Unknown, _app));
                _availableUnits.Add(new AmountUnitViewModel("ToTasteUnit", false, AmountUnit.ToTaste, _app));
                _availableUnits.Sort((u1, u2) => u1.UserVisibleName.CompareTo(u2.UserVisibleName));
            }
            return _availableUnits;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override Boolean Equals(Object obj)
        {
            AmountViewModel other = obj as AmountViewModel;
            if (other == null)
            {
                return false;
            }
            if (!other._unit.Equals(_unit) || Math.Abs(other._quantity - _quantity) < Double.Epsilon)
            {
                return false;
            }
            return true;
        }

        public override Int32 GetHashCode()
        {
            return _unit.GetHashCode() << 16 + _quantity.GetHashCode();
        }
    }
}
