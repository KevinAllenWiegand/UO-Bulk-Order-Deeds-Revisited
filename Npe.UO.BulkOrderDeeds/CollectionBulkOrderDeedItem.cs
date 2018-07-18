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
        private const string _IsCompletedCountAttributeName = "isCompleted";

        public string Name { get; }
        public int Quantity { get; }
        public bool IsCompleted { get; set; }

        public CollectionBulkOrderDeedItem(string name, int quantity)
            : this(name, quantity, false)
        {
        }

        public CollectionBulkOrderDeedItem(string name, int quantity, bool isCompleted)
        {
            Name = name;
            Quantity = quantity;
            IsCompleted = isCompleted;
        }

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_NameAttributeName, Name);
            writer.WriteAttributeString(_IsCompletedCountAttributeName, IsCompleted.ToString());
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
                        var isCompletedString = node.Attributes[_IsCompletedCountAttributeName].Value;

                        if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(isCompletedString))
                        {
                            var isCompleted = bool.Parse(isCompletedString);

                            retVal.Add(new CollectionBulkOrderDeedItem(name, quantity, isCompleted));
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
            var isCompletedString = IsCompleted ? " (Completed)" : String.Empty;

            return $"{Quantity} {Name}{isCompletedString}";
        }
    }
}
