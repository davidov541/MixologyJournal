using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.App;

namespace MixologyJournalApp.Test.Persistence
{
    public class MockAppPersister : BaseAppPersister
    {
        public override IPersistenceSource Source
        {
            get;
            set;
        }

        public MockAppPersister(BaseMixologyApp app) :
            base(app)
        {
        }

        public void AddTestSource(IPersistenceSource persister)
        {
            AddSource(persister);
        }

        protected override Task PortPictureToNewSourceAsync(string picture, IPersistenceSource oldSource)
		{
			throw new System.NotImplementedException();
		}
	}
}
