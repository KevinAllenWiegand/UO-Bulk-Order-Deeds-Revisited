
using Npe.UO.BulkOrderDeeds.Internal;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class PointReward
    {
        private const string _NameAttributeName = "name";
        private const string _PointsAttributeName = "points";
        private const string _IconAttributeName = "icon";

        public string Name { get; private set; }
        public int Points { get; private set; }
        public string Icon { get; private set; }

        internal PointReward(XmlNode xmlNode)
        {
            Name = XmlHelper.GetAttributeValue<string>(xmlNode, _NameAttributeName);
            Points = XmlHelper.GetAttributeValue<int>(xmlNode, _PointsAttributeName);
            Icon = XmlHelper.GetAttributeValue<string>(xmlNode, _IconAttributeName);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
