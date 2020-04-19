using MixologyJournalApp.Model;
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel: INotifyPropertyChanged
    {
        private Recipe _model;
        private List<StepViewModel> _steps;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public IEnumerable<StepViewModel> StepsList
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
            _steps = _model.Steps.Select((s, i) => new StepViewModel(s, i)).ToList();
            _steps.ForEach(s => s.PropertyChanged += stepChanged);
        }

        private void stepChanged(object sender, PropertyChangedEventArgs e)
        {
            StepViewModel vm = sender as StepViewModel;
            _model.Steps[vm.Index] = vm.Text;
        }
    }
}
