using MixologyJournalApp.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    internal class RecipeViewModel : INotifyPropertyChanged, IPictureCreation
    {
        private readonly Recipe _model;
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

        public String Id
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

        public String IngredientList
        {
            get
            {
                return String.Join(", ", IngredientUsages.Select(u => u.Ingredient.Name));
            }
        }

        public ImageSource Image
        {
            get
            {
                if (_model == null || _model.Picture == null)
                {
                    return ImageSource.FromFile("drawable/DefaultContentPic.png");
                }
                return _model.Picture.Image;
            }
        }

        public ObservableCollection<StepViewModel> Steps { get; } = new ObservableCollection<StepViewModel>();

        public ObservableCollection<IngredientUsageViewModel> IngredientUsages { get; } = new ObservableCollection<IngredientUsageViewModel>();

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

        public Boolean CanBeDeleted
        {
            get
            {
#if DEBUG
                return true;
#else
                return !_model.IsBuiltIn;
#endif
            }
        }

        public ICommand ToggleFavoriteCommand
        {
            get
            {
                return null;
            }
        }

        public Boolean CanBeFavorited
        {
            get
            {
                return false;
            }
        }

        public Boolean CanBeUnfavorited
        {
            get
            {
                return false;
            }
        }

        public Boolean HasReview
        {
            get
            {
                return false;
            }
        }

        public String Rating
        {
            get
            {
                return String.Empty;
            }
        }

        public String Review
        {
            get
            {
                return String.Empty;
            }
        }

        public CreationType Type
        {
            get
            {
                return CreationType.Recipe;
            }
        }

        public IEnumerable<DrinkViewModel> AssociatedDrinks
        {
            get
            {
                IEnumerable<DrinkViewModel> drinks = _app.Cache.Drinks.Where(d => d.BasisId.Equals(_model.Id));
                return drinks.OrderBy(d => !d.IsFavorite);
            }
        }

        public Boolean AnyDrinks
        {
            get
            {
                return AssociatedDrinks.Any();
            }
        }

        public RecipeViewModel(App app) : this(Recipe.CreateEmptyRecipe(), app)
        {
        }

        public RecipeViewModel(Recipe model, App app)
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

            PropertyChanged += RecipeViewModel_PropertyChanged;
        }

        private void RecipeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Steps):
                    OnPropertyChanged(nameof(FormattedSteps));

                    (DeleteStepCommand as Command).ChangeCanExecute();
                    break;
                case nameof(IngredientUsages):
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
            Boolean result = await _app.Cache.CreateRecipe(this, _model);
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

        public Drink CreateDerivedDrink()
        {
            return Drink.CreateEmptyDrink(_model);
        }

        public async Task Delete()
        {
            ProcessIsRunning = true;
            await _app.Cache.DeleteRecipe(_model);
            await _app.PopToRoot();
            ProcessIsRunning = false;
        }

        public async Task TakePicture()
        {
            ProcessIsRunning = true;
            StoreCameraMediaOptions options = new StoreCameraMediaOptions()
            {
                PhotoSize = PhotoSize.Medium,
                RotateImage = true,
            };
            MediaFile result = await CrossMedia.Current.TakePhotoAsync(options);
            if (result != null)
            {
                await _app.Cache.AddPicture(_model, result.Path);
                OnPropertyChanged(nameof(Image));
            }
            ProcessIsRunning = false;
        }

        public async Task ChoosePicture()
        {
            ProcessIsRunning = true;
            PickMediaOptions options = new PickMediaOptions()
            {
                RotateImage = true
            };
            MediaFile result = await CrossMedia.Current.PickPhotoAsync(options);
            if (result != null)
            {
                await _app.Cache.AddPicture(_model, result.Path);
                OnPropertyChanged(nameof(Image));
            }
            ProcessIsRunning = false;
        }
    }
}
