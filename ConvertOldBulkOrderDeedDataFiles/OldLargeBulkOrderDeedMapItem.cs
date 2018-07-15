using System;
using System.Collections.Generic;
using System.Xml;

namespace ConvertOldBulkOrderDeedDataFiles
{
    internal class OldLargeBulkOrderDeedMapItem
    {
        private const string _LargeBulkOrderDeedNameAttributeName = "LBODName";
        private const string _XMLNameAttributeName = "XMLName";
        private const string _CategoryAttributeName = "Category";

        public string BulkOrderDeedName { get; }
        public string XmlName { get; }
        public IEnumerable<string> Categories { get; }

        public OldLargeBulkOrderDeedMapItem(XmlNode xmlNode)
        {
            BulkOrderDeedName = XmlHelper.GetAttributeValue<string>(xmlNode, _LargeBulkOrderDeedNameAttributeName);
            XmlName = XmlHelper.GetAttributeValue<string>(xmlNode, _XMLNameAttributeName);

            var categoryString = XmlHelper.GetAttributeValue<string>(xmlNode, _CategoryAttributeName);

            Categories = categoryString.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            return BulkOrderDeedName;
        }
    }
}
