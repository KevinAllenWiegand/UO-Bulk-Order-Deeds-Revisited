
using Npe.UO.BulkOrderDeeds.Internal;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedMaterial
    {
        private const string _NameAttributeName = "name";

        public string Name { get; }

        internal BulkOrderDeedMaterial(XmlNode xmlNode)
        {
            Name = XmlHelper.GetAttributeValue<string>(xmlNode, _NameAttributeName);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
