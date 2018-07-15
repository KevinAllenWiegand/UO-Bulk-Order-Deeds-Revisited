using System;
using System.Collections.Generic;
using System.Xml;
using Npe.UO.BulkOrderDeeds.Internal;

namespace Npe.UO.BulkOrderDeeds
{
    public abstract class CollectionBulkOrderDeed
    {
        internal const string XmlRootName = "BulkOrderDeeds";

        private const string _XmlItemName = "BulkOrderDeed";
        private const string _IdAttributeName = "id";
        private const string _NameAttributeName = "name";
        private const string _TypeAttributeName = "type";
        private const string _ProfessionAttributeName = "profession";
        private const string _QuantityAttributeName = "quantity";
        private const string _ExceptionalAttributeName = "exceptional";
        private const string _MaterialAttributeName = "material";
        private const string _VendorAttributeName = "vendor";
        private const string _BulkOrderDeedBookAttributeName = "bulkOrderDeedBook";

        public Guid Id { get; }
        public string DisplayName { get { return BulkOrderDeedDefinition.DisplayName; } }
        public string Profession { get; }
        public BulkOrderDeedDefinition BulkOrderDeedDefinition { get; }
        public int Quantity { get; }
        public string Material { get; }
        public bool Exceptional { get; }
        public BulkOrderDeedLocation Location { get; }

        private readonly List<CollectionBulkOrderDeedItem> _CollectionBulkOrderDeedItems;
        public IReadOnlyCollection<CollectionBulkOrderDeedItem> CollectionBulkOrderDeedItems => _CollectionBulkOrderDeedItems.AsReadOnly();

        protected CollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
        {
            Guard.ArgumentNotNull(nameof(profession), profession);
            Guard.ArgumentNotNull(nameof(bulkOrderDeedDefinition), bulkOrderDeedDefinition);
            Guard.ArgumentAtLeast(nameof(quantity), 1, quantity);
            // Note that material CAN be null (for instance, for inscription bulk order deeds).

            Id = Guid.NewGuid();
            Profession = profession.Name;
            BulkOrderDeedDefinition = bulkOrderDeedDefinition;
            Quantity = quantity;
            Material = material != null ? material.Name : String.Empty;
            Exceptional = exceptional;
            _CollectionBulkOrderDeedItems = new List<CollectionBulkOrderDeedItem>();

            if (BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
            {
                _CollectionBulkOrderDeedItems.Add(new CollectionBulkOrderDeedItem(smallBulkOrderDeedDefinition.Name, Quantity));
            }

            if (BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
            {
                foreach (var smallBulkOrderDeed in largeBulkOrderDeedDefinition.SmallBulkOrderDeedDefinitions)
                {
                    _CollectionBulkOrderDeedItems.Add(new CollectionBulkOrderDeedItem(smallBulkOrderDeed.Name, Quantity));
                }
            }

            Location = new BulkOrderDeedLocation(Vendor.None, BulkOrderDeedBook.None);
        }

        protected CollectionBulkOrderDeed(string id, string profession, string bulkOrderDeedNameMatch, BulkOrderDeedType bulkOrderDeedType, int quantity, bool exceptional, string material)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(id), id);
            Guard.ArgumentNotNullOrEmpty(nameof(profession), profession);
            Guard.ArgumentNotNullOrEmpty(nameof(bulkOrderDeedNameMatch), bulkOrderDeedNameMatch);
            Guard.ArgumentNotOfValue(nameof(quantity), quantity, BulkOrderDeedManager.PossibleQuantities);
            // Note that material CAN be null (for instance, for inscription bulk order deeds).

            Id = Guid.Parse(id);
            Profession = profession;
            BulkOrderDeedDefinition = BulkOrderDeedManager.Instance.GetBulkOrderDeedDefinition(profession, bulkOrderDeedNameMatch, bulkOrderDeedType);
            Quantity = quantity;
            Material = material ?? String.Empty;
            Exceptional = exceptional;
            _CollectionBulkOrderDeedItems = new List<CollectionBulkOrderDeedItem>();

            if (BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
            {
                _CollectionBulkOrderDeedItems.Add(new CollectionBulkOrderDeedItem(smallBulkOrderDeedDefinition.Name, Quantity));
            }

            if (BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
            {
                foreach (var smallBulkOrderDeed in largeBulkOrderDeedDefinition.SmallBulkOrderDeedDefinitions)
                {
                    _CollectionBulkOrderDeedItems.Add(new CollectionBulkOrderDeedItem(smallBulkOrderDeed.Name, Quantity));
                }
            }

            Location = new BulkOrderDeedLocation(Vendor.None, BulkOrderDeedBook.None);

            // TODO:  This is here when loading from files...we need to also set the completed counts.
        }

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_IdAttributeName, Id.ToString());
            writer.WriteAttributeString(_TypeAttributeName, BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition ? "Small" : "Large");
            writer.WriteAttributeString(_ProfessionAttributeName, Profession);
            writer.WriteAttributeString(_NameAttributeName, DisplayName);
            writer.WriteAttributeString(_QuantityAttributeName, Quantity.ToString());
            writer.WriteAttributeString(_ExceptionalAttributeName, Exceptional.ToString());
            writer.WriteAttributeString(_MaterialAttributeName, Material);
            writer.WriteAttributeString(_VendorAttributeName, Location.Vendor.Id.ToString());
            writer.WriteAttributeString(_BulkOrderDeedBookAttributeName, Location.BulkOrderDeedBook.Id.ToString());
            writer.WriteStartElement(CollectionBulkOrderDeedItem.XmlRootName);

            foreach (var bulkOrderDeedItem in _CollectionBulkOrderDeedItems)
            {
                bulkOrderDeedItem.SaveToXml(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            string quality = Exceptional ? "Exceptional" : "Normal";
            string material = String.IsNullOrEmpty(Material) ? " " : $" {Material}";

            return $"{Quantity} {quality}{material} ({Profession})";
        }
    }
}
