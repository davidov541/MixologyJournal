using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MixologyJournal.View
{
    public class IsFavoriteToIconConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            if ((bool)value)
            {
                return new SymbolIcon(Symbol.UnFavorite);
            }
            else
            {
                return new SymbolIcon(Symbol.Favorite);
            }
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
