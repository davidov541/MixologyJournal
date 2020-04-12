using MixologyJournalApp.Model;
using System;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel
    {
        private Recipe _model;
        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public RecipeViewModel(Recipe model)
        {
            _model = model;
        }
    }
}
