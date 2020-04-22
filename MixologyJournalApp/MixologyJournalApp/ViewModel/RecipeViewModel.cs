﻿using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeViewModel: INotifyPropertyChanged
    {
        private readonly Recipe _model;

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

        public Boolean CanDeleteStep
        {
            get
            {
                return Steps.Count > 1;
            }
        }

        public Boolean CanDeleteIngredient
        {
            get
            {
                return IngredientUsages.Count > 1;
            }
        }

        public RecipeViewModel() : this(Recipe.CreateEmptyRecipe())
        {
        }

        public RecipeViewModel(Recipe model)
        {
            _model = model;

            IEnumerable<StepViewModel> steps = _model.Steps.Select((s, i) => new StepViewModel(s, i));
            foreach(StepViewModel s in steps)
            {
                s.PropertyChanged += StepChanged;
                Steps.Add(s);
            }

            IEnumerable<IngredientUsageViewModel> usages = _model.Ingredients.Select(u => new IngredientUsageViewModel(u));
            foreach (IngredientUsageViewModel u in usages)
            {
                IngredientUsages.Add(u);
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
            StepViewModel stepvm = new StepViewModel("", Steps.Count);
            stepvm.PropertyChanged += StepChanged;
            Steps.Add(stepvm);
            OnPropertyChanged(nameof(FormattedSteps));
            OnPropertyChanged(nameof(Steps));
            OnPropertyChanged(nameof(CanDeleteStep));
        }

        public void DeleteStep(StepViewModel step)
        {
            int index = step.Index;
            Steps.Remove(step);
            _model.Steps.RemoveAt(index);
            OnPropertyChanged(nameof(FormattedSteps));
            OnPropertyChanged(nameof(Steps));
            OnPropertyChanged(nameof(CanDeleteStep));
        }

        public void AddIngredient()
        {
            IngredientUsage usage = IngredientUsage.CreateEmpty();
            IngredientUsageViewModel viewModel = new IngredientUsageViewModel(usage);
            IngredientUsages.Add(viewModel);
            _model.Ingredients.Add(usage);
            OnPropertyChanged(nameof(IngredientUsages));
            OnPropertyChanged(nameof(FormattedIngredients));
            OnPropertyChanged(nameof(CanDeleteIngredient));
        }

        public void DeleteIngredient(IngredientUsageViewModel usage)
        {
            int index = IngredientUsages.IndexOf(usage);
            IngredientUsages.Remove(usage);
            _model.Ingredients.RemoveAt(index);
            OnPropertyChanged(nameof(IngredientUsages));
            OnPropertyChanged(nameof(FormattedIngredients));
            OnPropertyChanged(nameof(CanDeleteIngredient));
        }
    }
}
