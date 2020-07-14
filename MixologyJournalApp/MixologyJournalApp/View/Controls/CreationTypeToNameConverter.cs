using MixologyJournalApp.ViewModel;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class CreationTypeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CreationType type = (CreationType)value;
            switch(type)
            {
                case CreationType.Drink:
                    return "Drink";
                case CreationType.Recipe:
                    return "Recipe";
                default:
                    throw new NotSupportedException("Invalid creation type: " + type.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String typeName = (String)value;
            switch (typeName)
            {
                case "Drink":
                    return CreationType.Drink;
                case "Recipe":
                    return CreationType.Recipe;
                default:
                    throw new NotSupportedException("Invalid creation type name: " + typeName);
            }
        }
    }
}
