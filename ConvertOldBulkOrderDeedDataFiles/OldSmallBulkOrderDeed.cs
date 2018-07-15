using System;
using System.Collections.Generic;
using System.Xml;

namespace ConvertOldBulkOrderDeedDataFiles
{
    internal class OldSmallBulkOrderDeed
    {
        private const string _NameAttributeName = "Name";
        private const string _CategoryAttributeName = "Category";

        public string Name { get; }
        public IEnumerable<string> Categories { get; }

        public OldSmallBulkOrderDeed(XmlNode xmlNode)
        {
            Name = XmlHelper.GetAttributeValue<string>(xmlNode, _NameAttributeName);

            var categoryString = XmlHelper.GetAttributeValue<string>(xmlNode, _CategoryAttributeName);

            Categories = categoryString.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
