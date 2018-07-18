using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class LargeCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public override BulkOrderDeedType BulkOrderDeedType => BulkOrderDeedType.Large;

        private readonly List<CollectionBulkOrderDeedItem> _CollectionBulkOrderDeedItems;
        public IReadOnlyCollection<CollectionBulkOrderDeedItem> CollectionBulkOrderDeedItems => _CollectionBulkOrderDeedItems.AsReadOnly();

        public LargeCollectionBulkOrderDeed(Profession profession, LargeBulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, Vendor.None, BulkOrderDeedBook.None)
        {
            _CollectionBulkOrderDeedItems = new List<CollectionBulkOrderDeedItem>();
            AddCollectionBulkOrderDeedItems(bulkOrderDeedDefinition, quantity);
        }

        public LargeCollectionBulkOrderDeed(Profession profession, LargeBulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material, Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook, IDictionary<SmallBulkOrderDeedDefinition, bool> completedStates)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, vendor, bulkOrderDeedBook)
        {
            _CollectionBulkOrderDeedItems = new List<CollectionBulkOrderDeedItem>();
            AddCollectionBulkOrderDeedItems(bulkOrderDeedDefinition, quantity);

            foreach (var keyValuePair in completedStates)
            {
                var item = _CollectionBulkOrderDeedItems.FirstOrDefault(i => String.Compare(i.Name, keyValuePair.Key.Name, true) == 0);

                if (item != null)
                {
                    item.IsCompleted = keyValuePair.Value;
                }
            }
        }

        public LargeCollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook, IEnumerable<CollectionBulkOrderDeedItem> collectionBulkOrderDeedItems)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Large, quantity, exceptional, material, vendor, bulkOrderDeedBook)
        {
            _CollectionBulkOrderDeedItems = new List<CollectionBulkOrderDeedItem>(collectionBulkOrderDeedItems);
        }

        private void AddCollectionBulkOrderDeedItems(LargeBulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity)
        {
            foreach (var smallBulkOrderDeed in bulkOrderDeedDefinition.SmallBulkOrderDeedDefinitions)
            {
                _CollectionBulkOrderDeedItems.Add(new CollectionBulkOrderDeedItem(smallBulkOrderDeed.Name, quantity));
            }
        }

        internal static CollectionBulkOrderDeed LoadFromXml(XmlNode node, Guid id, string profession, string name, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook)
        {
            var bulkOrderDeedItems = CollectionBulkOrderDeedItem.LoadFromXml(node, quantity);

            return new LargeCollectionBulkOrderDeed(id, profession, name, quantity, exceptional, material, vendor, bulkOrderDeedBook, bulkOrderDeedItems);
        }

        protected override void SaveToXmlImpl(XmlWriter writer)
        {
            writer.WriteStartElement(CollectionBulkOrderDeedItem.XmlRootName);

            foreach (var bulkOrderDeedItem in _CollectionBulkOrderDeedItems)
            {
                bulkOrderDeedItem.SaveToXml(writer);
            }

            writer.WriteEndElement();
        }
    }
}
