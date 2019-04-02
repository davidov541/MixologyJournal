using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MixologyJournalApp.Test.Persistence
{
    public class PersisterTests
    {
        #region Saving Utilities
        protected void CompareXml(XDocument one, XDocument other)
        {
            IEnumerable<XElement> oneNodes = one.Nodes().OfType<XElement>();
            IEnumerable<XElement> otherNodes = other.Nodes().OfType<XElement>();
            Assert.AreEqual(oneNodes.Count(), otherNodes.Count());
            foreach (XElement oneNode in oneNodes)
            {
                XElement otherNode = otherNodes.SingleOrDefault(n => AreSameNodes(oneNode, n));
                Assert.IsNotNull(otherNode);
                CompareXNode(oneNode, otherNode);
            }
        }

        private void CompareXNode(XElement one, XElement other)
        {
            IEnumerable<XElement> oneNodes = one.Nodes().OfType<XElement>();
            IEnumerable<XElement> otherNodes = other.Nodes().OfType<XElement>();
            Assert.AreEqual(oneNodes.Count(), otherNodes.Count());
            foreach (XElement oneNode in oneNodes)
            {
                XElement otherNode = otherNodes.SingleOrDefault(n => AreSameNodes(oneNode, n));
                Assert.IsNotNull(otherNode);
                CompareXNode(oneNode, otherNode);
            }
        }

        private bool AreSameNodes(XElement one, XElement other)
        {
            if (!one.Name.LocalName.Equals(other.Name.LocalName))
            {
                return false;
            }
            IEnumerable<XAttribute> oneAttributes = one.Attributes();
            IEnumerable<XAttribute> otherAttributes = other.Attributes();
            if (oneAttributes.Count() != otherAttributes.Count())
            {
                return false;
            }
            foreach (XAttribute oneAttr in oneAttributes)
            {
                XAttribute otherAttr = otherAttributes.SingleOrDefault(a => a.Name.LocalName.Equals(oneAttr.Name.LocalName) && a.Value.Equals(oneAttr.Value));
                if (otherAttr == null)
                {
                    // FavoriteID can vary based on other tests, so as long as both have it, we can ignore the actual value.
                    if (oneAttr.Name.LocalName.EndsWith("ID") && other.Attributes().Any(a => a.Name.LocalName.Equals(oneAttr.Name.LocalName)))
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
