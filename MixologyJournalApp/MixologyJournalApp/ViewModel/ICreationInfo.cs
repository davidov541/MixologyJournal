using System;
using System.Collections.ObjectModel;

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

        String StepText
        {
            get;
        }

        ObservableCollection<IngredientUsageViewModel> IngredientUsages 
        {
            get; 
        }

        Boolean HasReview
        {
            get;
        }

        String Rating
        {
            get;
        }

        String Review
        {
            get;
        }

        CreationType Type
        {
            get;
        }
    }
}
