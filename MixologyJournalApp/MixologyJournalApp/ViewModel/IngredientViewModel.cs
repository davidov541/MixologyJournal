using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MixologyJournalApp.ViewModel
{
    internal class IngredientViewModel
    {
        private readonly Ingredient _model;
        public String Id
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

        public IngredientViewModel(Ingredient model)
        {
            _model = model;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
