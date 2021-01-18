using System;
using System.Globalization;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class ProgressBarCompleteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double doubleValue = (double)value;
            return doubleValue != 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
