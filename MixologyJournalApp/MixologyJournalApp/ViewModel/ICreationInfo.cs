using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MixologyJournalApp.ViewModel
{
    internal interface ICreationInfo
    {
        String Name
        {
            get;
        }

        String IngredientList
        {
            get;
        }

        String FormattedSteps
        {
            get;
        }

        ObservableCollection<IngredientUsageViewModel> IngredientUsages 
        {
            get; 
        }

        ICommand DeleteCommand
        {
            get;
        }

        Boolean CanBeDeleted
        {
            get;
        }

        ICommand ToggleFavoriteCommand
        {
            get;
        }

        Boolean CanBeFavorited
        {
            get;
        }

        Boolean CanBeUnfavorited
        {
            get;
        }
    }
}
