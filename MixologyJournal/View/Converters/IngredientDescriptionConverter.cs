using System;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class IngredientDescriptionConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            if (value == null)
            {
                return "New Ingredient";
            }
            return value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
