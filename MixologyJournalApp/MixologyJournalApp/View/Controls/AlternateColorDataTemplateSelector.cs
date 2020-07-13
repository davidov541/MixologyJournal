using System.Collections;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class AlternateColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(container is ListView listView))
            {
                return UnevenTemplate;
            }
            else
            {
                IList listItem = listView.ItemsSource as IList;

                int index = listItem.IndexOf(item);
                return index % 2 == 0 ? EvenTemplate : UnevenTemplate;
            }
        }
    }
}
