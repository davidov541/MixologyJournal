using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal class DrinkViewModel : INotifyPropertyChanged, ICreationInfo
    {
        private readonly Drink _model;
        private readonly App _app;

        public event PropertyChangedEventHandler PropertyChanged;

        public String Name
        {
            get
            {
                return _model.Name;
            }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        internal String Id
        {
            get
            {
                return _model.Id;
            }
        }

        public String FormattedSteps
        {
            get
            {
                int stepNum = 1;
                String result = "";
                foreach (String step in _model.Steps)
                {
                    result += stepNum.ToString() + ". " + step + "\n";
                    stepNum++;
                }
                return result;
            }
        }

        public ObservableCollection<StepViewModel> Steps { get; } = new ObservableCollection<StepViewModel>();

        public ObservableCollection<IngredientUsageViewModel> IngredientUsages { get; } = new ObservableCollection<IngredientUsageViewModel>();

        public String FormattedIngredients
        {
            get
            {
                return IngredientUsages.Select(i => i.ToString()).Aggregate((i1, i2) => i1 + "\n" + i2);
            }
        }

        public Boolean HasReview
        {
            get
            {
                return true;
            }
        }

        public String Rating
        {
            get
            {
                return String.Format("{0:0.0} Stars", _model.Rating);
            }
            set
            {
                float ratingVal = float.Parse(value.Split(' ')[0]);
                _model.Rating = ratingVal;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public String Review
        {
            get
            {
                return _model.Review.Replace("\\n", "\n");
            }
            set
            {
                _model.Review = value.Replace("\n", "\\n");
                OnPropertyChanged(nameof(Review));
            }
        }

        public String IngredientList
        {
            get
            {
                return String.Join(", ", IngredientUsages.Select(u => u.Ingredient.Name));
            }
        }

        public ICommand AddIngredientCommand
        {
            get;
            private set;
        }

        public ICommand DeleteIngredientCommand
        {
            get;
            private set;
        }

        public ICommand AddStepCommand
        {
            get;
            private set;
        }

        public ICommand DeleteStepCommand
        {
            get;
            private set;
        }

        public ICommand DeleteCommand
        {
            get;
            private set;
        }

        public Boolean CanBeDeleted
        {
            get
            {
                return true;
            }
        }

        public ICommand ToggleFavoriteCommand
        {
            get;
            private set;
        }

        public Boolean CanBeFavorited
        {
            get
            {
                return !IsFavorite;
            }
        }

        public Boolean CanBeUnfavorited
        {
            get
            {
                return IsFavorite;
            }
        }

        private Boolean _processIsRunning = false;
        public Boolean ProcessIsRunning
        {
            get
            {
                return _processIsRunning;
            }
            set
            {
                _processIsRunning = value;
                OnPropertyChanged(nameof(ProcessIsRunning));
            }
        }

        private Boolean _favoriteHasChanged = false;
        public Boolean IsFavorite
        {
            get
            {
                return _model.IsFavorite;
            }
            set
            {
                _model.IsFavorite = value;
                _favoriteHasChanged = !_favoriteHasChanged;
                OnPropertyChanged(nameof(IsFavorite));
                OnPropertyChanged(nameof(CanBeFavorited));
                OnPropertyChanged(nameof(CanBeUnfavorited));
                OnPropertyChanged(nameof(Type));
            }
        }

        public CreationType Type
        {
            get
            {
                if (IsFavorite)
                {
                    return CreationType.Favorite;
                }
                else
                {
                    return CreationType.Drink;
                }
            }
        }

        public String BasisId
        {
            get
            {
                return _model.SourceRecipeID;
            }
        }

        public DrinkViewModel(RecipeViewModel basis, App app) : this(basis.CreateDerivedDrink(), app)
        {
        }

        public DrinkViewModel(Drink model, App app)
        {
            _model = model;
            _app = app;

            InitCommands();

            IEnumerable<StepViewModel> steps = _model.Steps.Select((s, i) => new StepViewModel(s, i));
            foreach (StepViewModel s in steps)
            {
                s.PropertyChanged += StepChanged;
                Steps.Add(s);
            }

            IEnumerable<IngredientUsageViewModel> usages = _model.Ingredients.Select(u => new IngredientUsageViewModel(u, _app));
            foreach (IngredientUsageViewModel u in usages)
            {
                IngredientUsages.Add(u);
            }

            PropertyChanged += DrinkViewModel_PropertyChanged;
        }

        private void DrinkViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Steps):
                    OnPropertyChanged(nameof(FormattedSteps));

                    (DeleteStepCommand as Command).ChangeCanExecute();
                    break;
                case nameof(IngredientUsages):
                    OnPropertyChanged(nameof(FormattedIngredients));

                    (DeleteIngredientCommand as Command).ChangeCanExecute();
                    break;
            }
        }

        private void InitCommands()
        {
            AddIngredientCommand = new Command(
                execute: () =>
                {
                    AddIngredient();
                },
                canExecute: () =>
                {
                    return true;
                });
            DeleteIngredientCommand = new Command(
                execute: (ingred) =>
                {
                    DeleteIngredient(ingred as IngredientUsageViewModel);
                },
                canExecute: (ingred) =>
                {
                    return IngredientUsages.Count > 1;
                });
            AddStepCommand = new Command(
                execute: () =>
                {
                    AddStep();
                },
                canExecute: () =>
                {
                    return true;
                });
            DeleteStepCommand = new Command(
                execute: (step) =>
                {
                    DeleteStep(step as StepViewModel);
                },
                canExecute: (step) =>
                {
                    return Steps.Count > 1;
                });
            DeleteCommand = new Command(
                execute: async () =>
                {
                    await Delete();
                },
                canExecute: () =>
                {
                    return CanBeDeleted;
                });
            ToggleFavoriteCommand = new Command(
                execute: () =>
                {
                    IsFavorite = !IsFavorite;
                },
                canExecute: () =>
                {
                    return true;
                });
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StepChanged(object sender, PropertyChangedEventArgs e)
        {
            StepViewModel vm = sender as StepViewModel;
            _model.Steps[vm.Index] = vm.Text;

            OnPropertyChanged(nameof(Steps));
        }

        public async Task<bool> SaveNew()
        {
            ProcessIsRunning = true;
            Boolean result = await _app.Cache.CreateDrink(this, _model);
            ProcessIsRunning = false;
            return result;
        }

        public void AddStep()
        {
            _model.Steps.Add("");
            StepViewModel stepvm = new StepViewModel("", Steps.Count);
            stepvm.PropertyChanged += StepChanged;
            Steps.Add(stepvm);
            OnPropertyChanged(nameof(Steps));
        }

        public void DeleteStep(StepViewModel step)
        {
            int index = step.Index;
            Steps.Remove(step);
            _model.Steps.RemoveAt(index);

            OnPropertyChanged(nameof(Steps));
        }

        public void AddIngredient()
        {
            IngredientUsage usage = IngredientUsage.CreateEmpty();
            IngredientUsageViewModel viewModel = new IngredientUsageViewModel(usage, _app);
            IngredientUsages.Add(viewModel);
            _model.Ingredients.Add(usage);

            OnPropertyChanged(nameof(IngredientUsages));
        }

        public void DeleteIngredient(IngredientUsageViewModel usage)
        {
            int index = IngredientUsages.IndexOf(usage);
            IngredientUsages.Remove(usage);
            _model.Ingredients.RemoveAt(index);

            OnPropertyChanged(nameof(IngredientUsages));
        }

        public async Task Delete()
        {
            ProcessIsRunning = true;
            await _app.Cache.DeleteDrink(_model);
            await _app.PopToRoot();
            ProcessIsRunning = false;
        }

        internal async Task SaveChanges()
        {
            if (_favoriteHasChanged)
            {
                ProcessIsRunning = true;
                await _app.Cache.UpdateFavoriteDrink(_model, IsFavorite);
                ProcessIsRunning = false;
                _favoriteHasChanged = false;
            }
        }
    }
}
