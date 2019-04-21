using System.Collections.Generic;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel.LocationServices
{
    public interface INearbyPlacesProvider
    {
        Task<IEnumerable<INearbyPlace>> GetNearbyPlacesAsync();
    }
}
