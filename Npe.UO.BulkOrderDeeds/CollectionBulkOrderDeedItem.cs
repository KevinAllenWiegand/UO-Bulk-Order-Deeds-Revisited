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
        {
            Name = name;
            Quantity = quantity;
        }

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_NameAttributeName, Name);
            writer.WriteAttributeString(_CompletedCountAttributeName, CompletedCount.ToString());
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            return $"{CompletedCount}/{Quantity} {Name}";
        }
    }
}
