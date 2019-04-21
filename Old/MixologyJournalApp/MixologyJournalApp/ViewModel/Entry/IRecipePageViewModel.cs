namespace MixologyJournal.ViewModel.Entry
{
    public interface IRecipePageViewModel : IJournalEntryViewModel, ISaveablePageViewModel
    {
        void AddIngredient();
    }
}
