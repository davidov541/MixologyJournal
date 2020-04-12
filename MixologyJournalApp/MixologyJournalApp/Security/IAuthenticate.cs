using System.Threading.Tasks;

namespace MixologyJournalApp.Security
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
}
