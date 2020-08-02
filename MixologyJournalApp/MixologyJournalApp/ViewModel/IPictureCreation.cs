using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal interface IPictureCreation: ICreation, IPictureCreationInfo
    {
        Task TakePicture();

        Task ChoosePicture();
    }
}
