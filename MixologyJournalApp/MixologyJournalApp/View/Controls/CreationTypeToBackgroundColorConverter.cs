using MixologyJournalApp.ViewModel;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class CreationTypeToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CreationType type = (CreationType)value;
            Object color = null;
            switch (type)
            {
                case CreationType.Recipe:
                    App.Current.Resources.TryGetValue("RecipeCardBackground", out color);
                    break;
                case CreationType.Drink:
                    App.Current.Resources.TryGetValue("DrinkCardBackground", out color);
                    break;
                default:
                    throw new NotSupportedException("Invalid creation type: " + type.ToString());
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
