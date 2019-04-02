using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using FakeSymbol = MixologyJournal.ViewModel.Symbol;

namespace MixologyJournal.View
{
    public class SymbolConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            FakeSymbol fake = (FakeSymbol)value;
            // The numbers should be exactly the same, so just convert it to an int and then back.
            return (Symbol)(int)fake;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            Symbol real = (Symbol)value;
            // The numbers should be exactly the same, so just convert it to an int and then back.
            return (FakeSymbol)(int)real;
        }
    }
}
