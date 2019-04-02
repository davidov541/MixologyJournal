using System;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    internal class BarDrinkViewModel : RecipeViewModel<BarDrink>
    {
        public override String Caption
        {
            get
            {
                return String.Empty;
            }
        }

        public BarDrinkViewModel(BaseMixologyApp app) :
            base(new BarDrink(), app)
        {
        }

        public BarDrinkViewModel(BarDrink sourceModel, BaseMixologyApp app) :
            base(sourceModel, app)
        {
        }

        public override void Save()
        {
            base.Save();
        }

        protected override void Reset()
        {
            base.Reset();
        }

        public override Boolean Equals(Object obj)
        {
            BarDrinkViewModel other = obj as BarDrinkViewModel;
            if (other == null)
            {
                return false;
            }
            return base.Equals(obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
