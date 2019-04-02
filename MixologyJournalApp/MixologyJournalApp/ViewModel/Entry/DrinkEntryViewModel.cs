using System;
using System.Threading.Tasks;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournal.ViewModel.Entry
{
    public interface IDrinkEntryViewModel : IJournalEntryViewModel
    {
        double Rating
        {
            get;
            set;
        }

        IViewRecipeViewModel Drink
        {
            get;
        }

        void AddIngredient();
    }

    internal class DrinkEntryViewModel : JournalEntryViewModel, IDrinkEntryViewModel
    {
        private BaseMixologyApp _app;

        private double _rating;
        public double Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public override String Title
        {
            get
            {
                if (Drink != null)
                {
                    return Drink.Title;
                }
                return String.Empty;
            }
            set
            {
                if (Drink != null)
                {
                    _drink.SetTitle(value);
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private IRecipeViewModel _drink;
        public IViewRecipeViewModel Drink
        {
            get
            {
                return _drink;
            }
        }

        internal DrinkEntryViewModel(BaseMixologyApp app)
            : base(null, app)
        {
            _app = app;
        }

        protected void AttachToDrink(IRecipeViewModel drink)
        {
            _drink = drink;
        }

        public void AddIngredient()
        {
            _drink.AddIngredient(new EditIngredientViewModel(_app));
        }

        public override async Task SaveAsync()
        {
            await base.SaveAsync();
            _drink.Save();
            (Model as IDrinkEntry).Rating = Rating;
        }

        protected override void Reset()
        {
            base.Reset();
            _drink.Cancel();
            Rating = (Model as IDrinkEntry).Rating;
        }
    }
}
