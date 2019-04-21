using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournal.ViewModel
{
    public interface IJournalViewModel : INotifyPropertyChanged
    {
    }

    internal class JournalViewModel : IJournalViewModel
    {
        private Journal _journal;
        private List<JournalEntryViewModel> _entries;
        private List<BaseRecipePageViewModel> _recipes;
        private BaseMixologyApp _app;

        public event PropertyChangedEventHandler PropertyChanged;

        internal IEnumerable<JournalEntryViewModel> Entries
        {
            get
            {
                return _entries;
            }
        }

        internal IEnumerable<BaseRecipePageViewModel> BaseRecipes
        {
            get
            {
                return _recipes;
            }
        }

        internal Journal Model
        {
            get
            {
                return _journal;
            }
        }

        internal bool IsEmpty
        {
            get
            {
                return !BaseRecipes.Any() && !Entries.Any();
            }
        }

        internal JournalViewModel(Journal journal, BaseMixologyApp app)
        {
            _entries = new List<JournalEntryViewModel>();
            _recipes = new List<BaseRecipePageViewModel>();
            _journal = journal;
            _app = app;
            foreach (BaseRecipe recipe in journal.BaseRecipes)
            {
                BaseRecipePageViewModel vm = recipe.CreateViewModel(app);
                _entries.AddRange(vm.DerivedRecipes);
                _recipes.Add(vm);
            }
            foreach (JournalEntry entry in journal.Entries.OfType<JournalEntry>())
            {
                _entries.Add(entry.CreateViewModel(app, this));
            }
        }

        internal async Task AddBaseRecipeAsync(BaseRecipePageViewModel recipe)
        {
            _recipes.Add(recipe);
            _journal.AddRecipe((recipe.Recipe as BaseRecipeViewModel).Model);
            OnPropertyChanged(nameof(BaseRecipes));
            foreach (HomemadeDrinkEntryViewModel modifiedRecipe in recipe.DerivedRecipes)
            {
                AddEntryNoSave(modifiedRecipe);
            }
            await _app.Persister.SaveAsync(this);
        }

        internal async Task AddEntryAsync(JournalEntryViewModel entry)
        {
            AddEntryNoSave(entry);
            await _app.Persister.SaveAsync(this);
        }

        private void AddEntryNoSave(JournalEntryViewModel entry)
        {
            _entries.Add(entry);
            _journal.AddEntry(entry.Model);
            OnPropertyChanged(nameof(Entries));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal async Task RemoveRecipeAsync(IBaseRecipePageViewModel baseRecipe)
        {
            if (_recipes.Contains(baseRecipe))
            {
                BaseRecipePageViewModel viewModel = baseRecipe as BaseRecipePageViewModel;
                _journal.RemoveRecipe((viewModel.Recipe as BaseRecipeViewModel).Model);
                _recipes.Remove(baseRecipe as BaseRecipePageViewModel);
                foreach (HomemadeDrinkEntryViewModel modifiedRecipe in viewModel.DerivedRecipes)
                {
                    RemoveEntryNoSave(modifiedRecipe);
                }
                await _app.Persister.SaveAsync(this);
                OnPropertyChanged(nameof(BaseRecipes));
            }
        }

        internal async Task RemoveEntryAsync(IJournalEntryViewModel entry)
        {
            if (_entries.Contains(entry))
            {
                RemoveEntryNoSave(entry as JournalEntryViewModel);
                await _app.Persister.SaveAsync(this);
            }
        }

        private void RemoveEntryNoSave(JournalEntryViewModel entry)
        {
            _journal.RemoveEntry(entry.Model);
            _entries.Remove(entry);
            OnPropertyChanged(nameof(Entries));
        }

        internal BaseRecipePageViewModel GetRecipeByID(int id)
        {
            return _recipes.SingleOrDefault(r => r.ID == id);
        }
    }
}
