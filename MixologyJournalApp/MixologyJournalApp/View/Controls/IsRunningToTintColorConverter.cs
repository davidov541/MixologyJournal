using System;
using System.Globalization;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class IsRunningToTintColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double transparency = ((Boolean)value) ? 0xAA / 255.0 : 0.0;
            return new Color(0.0, 0.0, 0.0, transparency);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
