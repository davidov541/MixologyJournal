using System.Xml.Linq;
using MixologyJournal.SourceModel;

namespace MixologyJournal.Persistence
{
    internal interface ISettingsPersister<T>
    {
        void Write(T elementToPersist, XContainer parentNode);
        bool TryCreate(XElement element, Journal journal, out T createdElement);
    }
}
