using MixologyJournalApp.MAUI.ViewModel;


namespace MixologyJournalApp.MAUI.Views;

[QueryProperty("Recipe", "Recipe")]
public partial class RecipeViewPage : ContentPage
{
	public RecipeViewModel Recipe
	{
		get => BindingContext as RecipeViewModel;
		set => BindingContext = value;
	}
    
    private AppViewModel _appViewModel;

    public RecipeViewPage(AppViewModel appViewModel)
    {
        InitializeComponent();
        _appViewModel = appViewModel;
    }

    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}