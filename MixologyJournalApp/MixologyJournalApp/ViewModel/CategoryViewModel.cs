using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MixologyJournalApp.ViewModel
{
    internal class CategoryViewModel
    {
        private Category _model;

        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public IEnumerable<CategoryViewModel> Subcategories
        {
            get
            {
                return _model.Subcategories.Select(c => new CategoryViewModel(c)).OrderBy(c => c.Name);
            }
        }

        public IEnumerable<IngredientViewModel> Ingredients
        {
            get
            {
                return _model.Ingredients.Select(i => new IngredientViewModel(i)).OrderBy(i => i.Name);
            }
        }

        internal CategoryViewModel(Category model)
        {
            _model = model;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            CategoryViewModel other = obj as CategoryViewModel;
            return other != null && Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public Boolean ContainsIngredient(IngredientViewModel ingredient)
        {
            return Ingredients.Any(ingred => ingred.Equals(ingredient)) || Subcategories.Any(sub => sub.ContainsIngredient(ingredient));
        }
    }
}
