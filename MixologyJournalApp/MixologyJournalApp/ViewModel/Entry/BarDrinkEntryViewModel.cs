using System;
using System.Threading.Tasks;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournal.ViewModel.Entry
{
    public interface IBarDrinkEntryViewModel : IDrinkEntryViewModel
    {
        INearbyPlace Location
        {
            get;
            set;
        }
    }

    internal class BarDrinkEntryViewModel : DrinkEntryViewModel, IBarDrinkEntryViewModel
    {
        private BaseMixologyApp _app;

        public override String Caption
        {
            get
            {
                if (Location == null)
                {
                    return String.Empty;
                }
                return Location.Name;
            }
        }

        private INearbyPlace _location;
        public INearbyPlace Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        internal BarDrinkEntryViewModel(BaseMixologyApp app)
            : base(app)
        {
            _app = app;
            BarDrinkViewModel drink = new BarDrinkViewModel(app);
            AttachToDrink(drink);
            AttachToModel(new BarDrinkEntry(drink.Model));
        }

        internal BarDrinkEntryViewModel(BarDrinkEntry sourceModel, BaseMixologyApp app)
            : base(app)
        {
            _app = app;
            BarDrinkViewModel drink = new BarDrinkViewModel(sourceModel.Recipe, app);
            AttachToDrink(drink);
            AttachToModel(sourceModel);
        }

        public override async Task SaveAsync()
        {
            (Model as BarDrinkEntry).Location = Location;
            await base.SaveAsync();
        }

        protected override void Reset()
        {
            Location = (Model as BarDrinkEntry).Location;
            base.Reset();
        }
    }
}
