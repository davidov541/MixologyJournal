using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class StarVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            double rating = (double)value;
            double limit = Double.Parse(parameter.ToString());
            if (rating >= limit)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
