using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class ShowFavoriteConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
