﻿using MixologyJournalApp.MAUI.Model;
using System;

namespace MixologyJournalApp.MAUI.ViewModel
{
    public class IngredientViewModel
    {
        private readonly Ingredient _model;
        public int Id
        {
            get
            {
                return _model.Id;
            }
        }

        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public String Plural
        {
            get
            {
                return _model.Plural;
            }
        }

        internal Ingredient Model
        {
            get
            {
                return _model;
            }
        }

        internal IngredientViewModel(Ingredient model)
        {
            _model = model;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is IngredientViewModel other && other.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
