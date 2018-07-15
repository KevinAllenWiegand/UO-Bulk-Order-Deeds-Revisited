
using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public abstract class PointTableEntry
    {
        private const string _OverridesAttributeName = "overrides";
        private const string _MaterialAttributeName = "material";
        private const string _QuantityAttributeName = "quantity";
        private const string _ExceptionalAttributeName = "exceptional";
        private const string _PointsAttributeName = "points";

        public string Overrides { get; }
        public string Material { get; }
        public int Quantity { get; }
        public bool Exceptional { get; }
        public int Points { get; }

        internal PointTableEntry(XmlNode xmlNode)
        {
            Overrides = XmlHelper.GetAttributeValue<string>(xmlNode, _OverridesAttributeName);
            Material = XmlHelper.GetAttributeValue<string>(xmlNode, _MaterialAttributeName);
            Quantity = XmlHelper.GetAttributeValue<int>(xmlNode, _QuantityAttributeName);
            Exceptional = XmlHelper.GetAttributeValue<bool>(xmlNode, _ExceptionalAttributeName);
            Points = XmlHelper.GetAttributeValue<int>(xmlNode, _PointsAttributeName);
        }

        public override string ToString()
        {
            var quality = Exceptional ? "Exceptional" : "Normal";
            var overrides = !String.IsNullOrEmpty(Overrides) ? $" ({Overrides})" : "";

            return $"{Quantity} {quality} {Material}{overrides}";
        }
    }
}
