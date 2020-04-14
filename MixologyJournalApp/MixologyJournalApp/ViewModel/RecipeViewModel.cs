using MixologyJournalApp.Model;
using System;
using System.Linq;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel: INotifyPropertyChanged
    {
        private Recipe _model;

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

        public RecipeViewModel(Recipe model)
        {
            _model = model;
        }
    }
}
