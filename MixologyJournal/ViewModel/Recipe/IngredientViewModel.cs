using System;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    internal abstract class IngredientViewModel
    {
        private BaseMixologyApp _app;
        protected Ingredient _model;
        protected String _name;
        protected String _details;

        protected internal AmountViewModel InternalAmount
        {
            get;
            protected set;
        }

        internal Ingredient Model
        {
            get
            {
                return _model;
            }
        }

        protected IngredientViewModel(Ingredient model, BaseMixologyApp app)
        {
            _model = model;
            _app = app;
            _name = model.Name;
            _details = model.Details;
        }

        protected IngredientViewModel(IngredientViewModel ingredient) 
        {
            _model = ingredient._model;
            _app = ingredient._app;
            _name = ingredient._name;
            _details = ingredient._details;
        }
    }
}
