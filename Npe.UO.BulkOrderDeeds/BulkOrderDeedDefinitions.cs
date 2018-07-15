using Npe.UO.BulkOrderDeeds.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedDefinitions
    {
        private const string _SmallEntryXPath = "BulkOrderDeeds/Small/BulkOrderDeed";
        private const string _LargeEntryXPath = "BulkOrderDeeds/Large/BulkOrderDeed";

        private readonly List<BulkOrderDeedDefinition> _Definitions;

        public IEnumerable<BulkOrderDeedDefinition> Definitions => _Definitions.AsReadOnly();

        internal BulkOrderDeedDefinitions(string path)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            var smallEntryNodes = xmlDocument.SelectNodes(_SmallEntryXPath);

            _Definitions = new List<BulkOrderDeedDefinition>();

            if (smallEntryNodes != null)
            {
                foreach (var smallEntryNode in smallEntryNodes.OfType<XmlNode>())
                {
                    _Definitions.Add(new SmallBulkOrderDeedDefinition(smallEntryNode));
                }
            }

            var largeEntryNodes = xmlDocument.SelectNodes(_LargeEntryXPath);

            if (largeEntryNodes != null)
            {
                foreach (var largeEntryNode in largeEntryNodes.OfType<XmlNode>())
                {
                    _Definitions.Add(new LargeBulkOrderDeedDefinition(largeEntryNode));
                }
            }

            _Definitions.Sort(new BulkOrderDeedDefinitionComparer());
        }
    }
}
