using MixologyJournalApp.Model;
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel: INotifyPropertyChanged
    {
        private Recipe _model;
        private ObservableCollection<StepViewModel> _steps;

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

        public ObservableCollection<StepViewModel> StepsList
        {
            get
            {
                return _steps;
            }
        }

        public String Ingredients
        {
            get
            {
                String result = "";
                foreach (Ingredient ingred in _model.Ingredients)
                {
                    result += String.Format("{0} {1}s of {2}\n", ingred.Amount, ingred.Unit, ingred.Name);
                }
                return result;
            }
        }

        public RecipeViewModel() : this(new Recipe())
        {
        }

        public RecipeViewModel(Recipe model)
        {
            _model = model;
            List<StepViewModel> steps = _model.Steps.Select((s, i) => new StepViewModel(s, i)).ToList();
            steps.ForEach(s => s.PropertyChanged += StepChanged);
            _steps = new ObservableCollection<StepViewModel>(steps);
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
            StepViewModel stepvm = new StepViewModel("", _steps.Count);
            stepvm.PropertyChanged += StepChanged;
            _steps.Add(stepvm);
            OnPropertyChanged(nameof(Steps));
            OnPropertyChanged(nameof(StepsList));
        }
    }
}
