using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.List;

namespace MixologyJournal.ViewModel.Entry
{
    public interface IOverviewPageViewModel : IPageViewModel
    {
        IEnumerable<IListEntry> Entries
        {
            get;
        }

        IEnumerable<IListEntry> Recipes
        {
            get;
        }

        int MinimumEntriesPerGrouping
        {
            get;
            set;
        }

        Task<IBaseRecipePageViewModel> CreateBaseRecipeAsync();

        Task<INotesEntryViewModel> CreateTextJournalEntryAsync();

        ISettingsPageViewModel CreateSettingsPage();

        Task<IBarDrinkEntryViewModel> CreateBarDrinkPageAsync();
    }

    internal class OverviewPageViewModel : IOverviewPageViewModel
    {
        private BaseMixologyApp _app;
        private List<IListEntry> _entries;
        private List<IListEntry> _recipes;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<IListEntry> Entries
        {
            get
            {
                if (_entries == null)
                {
                    InitializeEntries();
                }
                return _entries;
            }
        }

        public IEnumerable<IListEntry> Recipes
        {
            get
            {
                if (_recipes == null)
                {
                    InitializeRecipes();
                }
                return _recipes;
            }
        }

        public bool AnyBaseRecipes
        {
            get
            {
                return Recipes.Any();
            }
        }

        public bool AnyJournalEntries
        {
            get
            {
                return Entries.Any();
            }
        }

        public bool Busy
        {
            get;
            private set;
        }

        private int _minimumEntriesPerGrouping = 1;
        public int MinimumEntriesPerGrouping
        {
            get
            {
                return _minimumEntriesPerGrouping;
            }
            set
            {
                _minimumEntriesPerGrouping = value;
                // Reset the entries and recipes so that the next time we query it, it will use the correct grouping.
                _recipes = null;
                _entries = null;
            }
        }

        public OverviewPageViewModel(BaseMixologyApp app)
        {
            _app = app;
            _app.Journal.PropertyChanged += Journal_PropertyChanged;
            MinimumEntriesPerGrouping = 1;
        }

        private void Journal_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(JournalViewModel.Entries)))
            {
                _entries = null;
                OnPropertyChanged(nameof(Entries));
                OnPropertyChanged(nameof(AnyJournalEntries));
            }
            else if (e.PropertyName.Equals(nameof(JournalViewModel.BaseRecipes)))
            {
                _recipes = null;
                OnPropertyChanged(nameof(Recipes));
                OnPropertyChanged(nameof(AnyBaseRecipes));
            }
        }

        private void InitializeEntries()
        {
            _entries = new List<IListEntry>(_app.Journal.Entries.OfType<IListEntry>());
            OnPropertyChanged(nameof(Entries));
            OnPropertyChanged(nameof(AnyJournalEntries));
        }

        private void InitializeRecipes()
        {
            _recipes = new List<IListEntry>();
            List<IGrouping<string, IListEntry>> combinedGroupings = CreateEntryGroupings(_app.Journal.BaseRecipes);
            foreach (IGrouping<String, IListEntry> grouping in combinedGroupings.Where(g => g != null))
            {
                if (MinimumEntriesPerGrouping >= 0)
                {
                    _recipes.Add(new LetterHeader(grouping.Key));
                }
                foreach (BaseRecipePageViewModel recipe in grouping.OfType<BaseRecipePageViewModel>())
                {
                    _recipes.Add(recipe);
                }
            }
            OnPropertyChanged(nameof(Recipes));
            OnPropertyChanged(nameof(AnyBaseRecipes));
        }

        private List<IGrouping<string, IListEntry>> CreateEntryGroupings(IEnumerable<IListEntry> listEntries)
        {
            IOrderedEnumerable<IGrouping<String, IListEntry>> groupings = listEntries.GroupBy(e => e.Title.Length > 0 ? e.Title[0].ToString() : " ").OrderBy(g => g.Key);
            List<IGrouping<String, IListEntry>> combinedGroupings = new List<IGrouping<string, IListEntry>>();
            IGrouping<String, IListEntry> currentGrouping = null;
            foreach (IGrouping<String, IListEntry> grouping in groupings)
            {
                IGrouping<String, IListEntry> newGrouping = null;
                if (currentGrouping == null)
                {
                    newGrouping = grouping;
                }
                else
                {
                    String newHeader = currentGrouping.Key[0] + " - " + grouping.Key;
                    IEnumerable<IListEntry> combinedEnumerable = currentGrouping.Union(grouping);
                    newGrouping = combinedEnumerable.GroupBy(e => newHeader).First();
                }
                if (newGrouping.Count() > MinimumEntriesPerGrouping)
                {
                    combinedGroupings.Add(newGrouping);
                    currentGrouping = null;
                }
                else
                {
                    currentGrouping = newGrouping;
                }
            }
            combinedGroupings.Add(currentGrouping);
            return combinedGroupings;
        }

        public async Task<IBaseRecipePageViewModel> CreateBaseRecipeAsync()
        {
            BaseRecipePageViewModel recipe = new BaseRecipePageViewModel(_app);
            await _app.Journal.AddBaseRecipeAsync(recipe);
            return recipe;
        }

        public async Task<INotesEntryViewModel> CreateTextJournalEntryAsync()
        {
            NotesEntryViewModel entry = new NotesEntryViewModel(_app);
            await _app.Journal.AddEntryAsync(entry);
            return entry;
        }

        public ISettingsPageViewModel CreateSettingsPage()
        {
            return new SettingsPageViewModel(_app);
        }

        public async Task<IBarDrinkEntryViewModel> CreateBarDrinkPageAsync()
        {
            BarDrinkEntryViewModel entry = new BarDrinkEntryViewModel(_app);
            await _app.Journal.AddEntryAsync(entry);
            return entry;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
