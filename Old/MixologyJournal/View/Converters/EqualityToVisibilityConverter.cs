using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class EqualityToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            Object typedParameter = Enum.ToObject(value.GetType(), parameter);
            bool shouldBeVisible = (value == null && parameter == null) || (value != null && value.Equals(typedParameter));
            return shouldBeVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return parameter;
            }
            else
            {
                return null;
            }
        }
    }
}
