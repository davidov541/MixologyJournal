using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class HalfStarConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            double rating = (double)value;
            if (Math.Ceiling(rating) == Math.Floor(rating))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
