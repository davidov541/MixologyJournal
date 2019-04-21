using System;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class ServingsNumberConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            // Converting to drop down index.
            int val = (int)value;
            return val - 1;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            // Converting to servings number.
            int val = (int)value;
            return val + 1;
        }
    }
}
