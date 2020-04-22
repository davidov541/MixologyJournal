using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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

        public ObservableCollection<IngredientUsageViewModel> IngredientUsages { get; } = new ObservableCollection<IngredientUsageViewModel>();

        public String Ingredients
        {
            get
            {
                return IngredientUsages.Select(i => i.ToString()).Aggregate((i1, i2) => i1 + "\n" + i2);
            }
        }

        public Boolean CanDeleteStep
        {
            get
            {
                return StepsList.Count > 1;
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

            IEnumerable<IngredientUsageViewModel> usages = _model.Ingredients.Select(u => new IngredientUsageViewModel(u));
            foreach (IngredientUsageViewModel u in usages)
            {
                u.PropertyChanged += UsageChanged;
                IngredientUsages.Add(u);
            }
        }

        private void UsageChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
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
