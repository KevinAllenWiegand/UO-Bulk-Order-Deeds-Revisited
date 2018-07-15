using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class CollectionBulkOrderDeedItem
    {
        internal const string XmlRootName = "BulkOrderDeedItems";

        private const string _XmlItemName = "BulkOrderDeedItem";
        private const string _NameAttributeName = "name";
        private const string _CompletedCountAttributeName = "completedCount";

        public string Name { get; }
        public int CompletedCount { get; set; }
        public int Quantity { get; }

        public CollectionBulkOrderDeedItem(string name, int quantity)
            : this(name, quantity, 0)
        {
        }

        public CollectionBulkOrderDeedItem(string name, int quantity, int completedCount)
        {
            Name = name;
            Quantity = quantity;
            CompletedCount = completedCount;
        }

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_NameAttributeName, Name);
            writer.WriteAttributeString(_CompletedCountAttributeName, CompletedCount.ToString());
            writer.WriteEndElement();
        }

        internal static IEnumerable<CollectionBulkOrderDeedItem> LoadFromXml(XmlNode rootNode, int quantity)
        {
            var retVal = new List<CollectionBulkOrderDeedItem>();
            var nodes = rootNode.SelectNodes($"{XmlRootName}/{_XmlItemName}");

            if (nodes != null)
            {
                foreach (var node in nodes.OfType<XmlNode>())
                {
                    try
                    {
                        var name = node.Attributes[_NameAttributeName].Value;
                        var completedCountString = node.Attributes[_CompletedCountAttributeName].Value;

                        if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(completedCountString))
                        {
                            var completedCount = int.Parse(completedCountString);

                            retVal.Add(new CollectionBulkOrderDeedItem(name, quantity, completedCount));
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return retVal;
        }

        public override string ToString()
        {
            return $"{CompletedCount}/{Quantity} {Name}";
        }
    }
}
