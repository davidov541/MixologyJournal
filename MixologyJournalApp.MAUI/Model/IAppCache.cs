namespace MixologyJournalApp.MAUI.Model
{
    internal interface IAppCache
    {
        Recipe GetRecipeById(String id);
        
        Unit GetUnitById(String id);
        
        Ingredient GetIngredientById(String id);
    }
}
