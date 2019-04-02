using System;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class IngredientDescriptionToFontStyleConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            if (value == null)
            {
                return FontStyle.Italic;
            }
            return FontStyle.Normal;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
