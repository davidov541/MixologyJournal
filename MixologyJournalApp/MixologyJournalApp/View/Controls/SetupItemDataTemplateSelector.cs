using MixologyJournalApp.ViewModel;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class SetupItemDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate DescriptionTemplate { get; set; }
        public DataTemplate LoginTemplate { get; set; }
        public DataTemplate ImageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (((SetupPageItem)item).Type)
            {
                case SetupPageItem.ItemType.Description:
                    return DescriptionTemplate;
                case SetupPageItem.ItemType.Image:
                    return ImageTemplate;
                case SetupPageItem.ItemType.Login:
                    return LoginTemplate;
            }
            return null;
        }
    }
}
