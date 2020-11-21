﻿using MixologyJournalApp.Model;
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
    internal class DrinkViewModel : INotifyPropertyChanged, IPictureCreation
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

        public ObservableCollection<IngredientUsageViewModel> IngredientUsages { get; } = new ObservableCollection<IngredientUsageViewModel>();

        public String FormattedIngredients
        {
            get
            {
                return IngredientUsages.Select(i => i.ToString()).Aggregate((i1, i2) => i1 + "\n" + i2);
            }
        }

        public String StepText
        {
            get;
            set;
        }

        private Boolean _isDisplayed = true;
        public Boolean IsDisplayed
        {
            get
            {
                return _isDisplayed;
            }
            private set
            {
                _isDisplayed = value;
                OnPropertyChanged(nameof(IsDisplayed));
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

        public ICommand UpdateStepsCommand
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
                if (value && !IsFavorite)
                {
                    foreach (DrinkViewModel other in _app.Cache.Drinks.Where(d => d.BasisId.Equals(BasisId) && !d.Id.Equals(Id) && d.IsFavorite))
                    {
                        other.IsFavorite = false;
                    }
                }
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

            StepText = String.Join("\n", _model.Steps);

            IEnumerable<IngredientUsageViewModel> usages = _model.Ingredients.Select(u => new IngredientUsageViewModel(u));
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
            UpdateStepsCommand = new Command(
                execute: () =>
                {
                    UpdateSteps();
                },
                canExecute: () =>
                {
                    return true;
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

        private void UpdateSteps()
        {
            _model.Steps = this.StepText.Split('\n').ToList();
        }

        public async Task<bool> SaveNew()
        {
            ProcessIsRunning = true;
            Boolean result = await _app.Cache.CreateDrink(this, _model);
            ProcessIsRunning = false;
            return result;
        }

        public void AddIngredient()
        {
            IngredientUsage usage = IngredientUsage.CreateEmpty();
            IngredientUsageViewModel viewModel = new IngredientUsageViewModel(usage);
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

        public void ApplySearchParameter(String searchTerm)
        {
            IsDisplayed = Name.Contains(searchTerm);
        }
    }
}
