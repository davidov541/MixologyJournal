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

        public MockAppPersister(IMixologyApp app) :
            base(app)
        {
        }

        public new void AddSource(IPersistenceSource persister)
        {
            base.AddSource(persister);
        }
    }
}
