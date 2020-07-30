using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal interface IPictureCreationInfo: ICreationInfo
    {
        ImageSource Image
        {
            get;
        }
    }
}
