using System;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    internal class RatingToStringConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            return String.Format("{0:N2}", (double)value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            return Double.Parse(value.ToString());
        }
    }
}
