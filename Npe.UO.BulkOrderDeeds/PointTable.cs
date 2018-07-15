using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class PointTable
    {
        private const string _SmallEntryXPath = "PointTable/Small/Entry";
        private const string _LargeEntryXPath = "PointTable/Large/Entry";

        private readonly List<PointTableEntry> _PointTableEntries;

        public IEnumerable<PointTableEntry> PointTableEntries => _PointTableEntries.AsReadOnly();

        internal PointTable(string path)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            var smallEntryNodes = xmlDocument.SelectNodes(_SmallEntryXPath);

            _PointTableEntries = new List<PointTableEntry>();

            if (smallEntryNodes != null)
            {
                foreach (var smallEntryNode in smallEntryNodes.OfType<XmlNode>())
                {
                    _PointTableEntries.Add(new SmallBulkOrderPointTableEntry(smallEntryNode));
                }
            }

            var largeEntryNodes = xmlDocument.SelectNodes(_LargeEntryXPath);

            if (largeEntryNodes != null)
            {
                foreach (var largeEntryNode in largeEntryNodes.OfType<XmlNode>())
                {
                    _PointTableEntries.Add(new LargeBulkOrderPointTableEntry(largeEntryNode));
                }
            }
        }

        public PointTable()
        {
        }
    }
}
