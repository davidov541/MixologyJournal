using System;
using System.Linq;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournal.ViewModel
{
    public static class SourceModelExtensions
    {
        internal static JournalEntryViewModel CreateViewModel(this JournalEntry entry, BaseMixologyApp app, IJournalViewModel journal)
        {
            JournalViewModel viewModel = journal as JournalViewModel;
            if (entry is NoteEntry)
            {
                return new NotesEntryViewModel(entry as NoteEntry, app);
            }
            else if (entry is HomemadeDrinkEntry)
            {
                HomemadeDrinkEntry model = entry as HomemadeDrinkEntry;
                BaseRecipePageViewModel recipePage = viewModel.BaseRecipes.OfType<BaseRecipePageViewModel>().FirstOrDefault(r => (r.Recipe as BaseRecipeViewModel).Model.Equals(model.Recipe.BaseRecipe));
                HomemadeDrinkEntryViewModel modifiedPage = new HomemadeDrinkEntryViewModel(model, recipePage, app);
                recipePage.AddModifiedRecipe(modifiedPage);
                return modifiedPage;
            }
            else if (entry is BarDrinkEntry)
            {
                return new BarDrinkEntryViewModel(entry as BarDrinkEntry, app);
            }
            throw new NotImplementedException();
        }

        internal static BaseRecipePageViewModel CreateViewModel(this BaseRecipe recipe, BaseMixologyApp app)
        {
            return new BaseRecipePageViewModel(new BaseRecipeViewModel(recipe, app), app);
        }
    }
}
