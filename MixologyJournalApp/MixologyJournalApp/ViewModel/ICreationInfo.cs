﻿using System;
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

        String FormattedSteps
        {
            get;
        }

        ObservableCollection<IngredientUsageViewModel> IngredientUsages 
        {
            get; 
        }
    }
}
