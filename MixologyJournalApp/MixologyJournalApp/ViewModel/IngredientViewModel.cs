using MixologyJournalApp.Model;
using System;

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

        internal Ingredient Model
        {
            get
            {
                return _model;
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

        public override bool Equals(object obj)
        {
            IngredientViewModel other = obj as IngredientViewModel;
            if (other == null)
            {
                return false;
            }
            return other.Id.Equals(Id);
        }
    }
}
