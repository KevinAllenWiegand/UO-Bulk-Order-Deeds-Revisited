
using Npe.UO.BulkOrderDeeds.Internal;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class LargeBulkOrderPointTableEntry : PointTableEntry
    {
        private const string _TypeAttributeName = "type";

        public string BulkOrderDeedType { get; }

        internal LargeBulkOrderPointTableEntry(XmlNode xmlNode)
            : base(xmlNode)
        {
            BulkOrderDeedType = XmlHelper.GetAttributeValue<string>(xmlNode, _TypeAttributeName);
        }
    }
}
