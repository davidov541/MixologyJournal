﻿using MixologyJournalApp.Model;
using System;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class IngredientUsageViewModel : INotifyPropertyChanged
    {
        internal struct State
        {
            public IngredientViewModel Ingredient
            {
                get;
                set;
            }

            public String Amount
            {
                get;
                set;
            }

            public String Brand
            {
                get;
                set;
            }

            public UnitViewModel Unit
            {
                get;
                set;
            }

            internal State(IngredientUsageViewModel viewModel)
            {
                Ingredient = viewModel.Ingredient;
                Amount = viewModel.Amount;
                Unit = viewModel.Unit;
                Brand = viewModel.Brand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IngredientUsage _model;

        private IngredientViewModel _ingredient;
        public IngredientViewModel Ingredient
        {
            get
            {
                return _ingredient;
            }
            set
            {
                _ingredient = value;
                if (_ingredient != null)
                {
                    _model.Ingredient = _ingredient.Model;
                }
                OnPropertyChanged(nameof(Ingredient));
                OnPropertyChanged(nameof(FullDescription));
            }
        }

        public String FormattedAmount
        {
            get
            {
                return Amount + " " + _unit.ToString();
            }
        }

        public String Brand
        {
            get
            {
                return _model.Brand;
            }
            set
            {
                _model.Brand = value;
                OnPropertyChanged(nameof(Brand));
                OnPropertyChanged(nameof(FullDescription));
            }
        }

        public String Amount
        {
            get
            {
                return _model.Amount;
            }
            set
            {
                _model.Amount = value;
                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(FullDescription));
            }
        }

        private UnitViewModel _unit;
        public UnitViewModel Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                if (_unit != null)
                {
                    _model.Unit = _unit.Model;
                }
                OnPropertyChanged(nameof(Unit));
                OnPropertyChanged(nameof(FullDescription));
            }
        }

        public String FullDescription
        {
            get
            {
                return ToString();
            }
        }

        public IngredientUsageViewModel(IngredientUsage model)
        {
            _model = model;

            Ingredient = new IngredientViewModel(_model.Ingredient);
            Unit = new UnitViewModel(_model.Unit);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Amount) || Unit == null || String.IsNullOrEmpty(Unit.Name) || Ingredient == null || String.IsNullOrEmpty(Ingredient.Name))
            {
                return String.Empty;
            }
            else if (Double.Parse(Amount) == 1.0)
            {
                return String.Format(Unit.Format, Unit.SingularArticle, Unit.Name, GetIngredientDescription(Ingredient.Name, Brand));
            }
            else
            {
                return String.Format(Unit.Format, Amount, Unit.Plural, GetIngredientDescription(Ingredient.Plural, Brand));
            }
        }

        private static String GetIngredientDescription(String name, String brand)
        {
            if (String.IsNullOrEmpty(brand))
            {
                return name;
            }
            else
            {
                return String.Format("{0} {1}", brand, name);
            }
        }

        public void RestoreFromState(State state)
        {
            Ingredient = state.Ingredient;
            Unit = state.Unit;
            Amount = state.Amount;
            Brand = state.Brand;
        }
    }
}
