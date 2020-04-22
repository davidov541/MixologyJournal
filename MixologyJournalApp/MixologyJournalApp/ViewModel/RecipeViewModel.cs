using MixologyJournalApp.Model;
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel: INotifyPropertyChanged
    {
        private Recipe _model;
        private LocalDataCache _cache;

        public event PropertyChangedEventHandler PropertyChanged;

        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public String Steps
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

        public ObservableCollection<StepViewModel> StepsList { get; } = new ObservableCollection<StepViewModel>();

        public String Ingredients
        {
            get
            {
                String result = "";
                foreach (IngredientUsage ingred in _model.Ingredients)
                {
                    result += String.Format("{0} {1}s of {2}\n", ingred.Amount, ingred.Unit, ingred.Name);
                }
                return result;
            }
        }

        public Boolean CanDeleteStep
        {
            get
            {
                return StepsList.Count > 1;
            }
        }

        public ObservableCollection<IngredientViewModel> AvailableIngredients 
        {
            get
            {
                return _cache.AvailableIngredients;
            }
        }

        public RecipeViewModel() : this(Recipe.CreateEmptyRecipe())
        {
        }

        public RecipeViewModel(Recipe model)
        {
            _model = model;
            _cache = App.GetInstance().Cache;

            IEnumerable<StepViewModel> steps = _model.Steps.Select((s, i) => new StepViewModel(s, i));
            foreach(StepViewModel s in steps)
            {
                s.PropertyChanged += StepChanged;
                StepsList.Add(s);
            }
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StepChanged(object sender, PropertyChangedEventArgs e)
        {
            StepViewModel vm = sender as StepViewModel;
            _model.Steps[vm.Index] = vm.Text;
        }

        public void AddStep()
        {
            _model.Steps.Add("");
            StepViewModel stepvm = new StepViewModel("", StepsList.Count);
            stepvm.PropertyChanged += StepChanged;
            StepsList.Add(stepvm);
            OnPropertyChanged(nameof(Steps));
            OnPropertyChanged(nameof(StepsList));
            OnPropertyChanged(nameof(CanDeleteStep));
        }

        public void DeleteStep(StepViewModel step)
        {
            int index = step.Index;
            StepsList.Remove(step);
            _model.Steps.RemoveAt(index);
            OnPropertyChanged(nameof(Steps));
            OnPropertyChanged(nameof(StepsList));
            OnPropertyChanged(nameof(CanDeleteStep));
        }
    }
}
