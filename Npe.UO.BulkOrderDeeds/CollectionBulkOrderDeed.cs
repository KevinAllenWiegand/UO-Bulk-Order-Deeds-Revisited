using System;
using System.Collections.Generic;
using System.Linq;
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
        public abstract BulkOrderDeedType BulkOrderDeedType { get; }

        protected CollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material, Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook)
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
            Location = new BulkOrderDeedLocation(vendor, bulkOrderDeedBook);
        }

        protected CollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, BulkOrderDeedType bulkOrderDeedType, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook)
        {
            Guard.ArgumentNotEmpty(nameof(id), id);
            Guard.ArgumentNotNullOrEmpty(nameof(profession), profession);
            Guard.ArgumentNotNullOrEmpty(nameof(bulkOrderDeedNameMatch), bulkOrderDeedNameMatch);
            Guard.ArgumentNotOfValue(nameof(quantity), quantity, BulkOrderDeedManager.PossibleQuantities);
            // Note that material CAN be null (for instance, for inscription bulk order deeds).

            Id = id;
            Profession = profession;
            BulkOrderDeedDefinition = BulkOrderDeedManager.Instance.GetBulkOrderDeedDefinition(profession, bulkOrderDeedNameMatch, bulkOrderDeedType);
            Quantity = quantity;
            Material = material ?? String.Empty;
            Exceptional = exceptional;
            Location = new BulkOrderDeedLocation(Vendor.None, BulkOrderDeedBook.None);
        }

        protected abstract void SaveToXmlImpl(XmlWriter writer);

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_IdAttributeName, Id.ToString());
            writer.WriteAttributeString(_TypeAttributeName, BulkOrderDeedType.ToString());
            writer.WriteAttributeString(_ProfessionAttributeName, Profession);
            writer.WriteAttributeString(_NameAttributeName, DisplayName);
            writer.WriteAttributeString(_QuantityAttributeName, Quantity.ToString());
            writer.WriteAttributeString(_ExceptionalAttributeName, Exceptional.ToString());
            writer.WriteAttributeString(_MaterialAttributeName, Material);
            writer.WriteAttributeString(_VendorAttributeName, Location.Vendor.Id.ToString());
            writer.WriteAttributeString(_BulkOrderDeedBookAttributeName, Location.BulkOrderDeedBook.Id.ToString());

            SaveToXmlImpl(writer);

            writer.WriteEndElement();
        }

        internal static IEnumerable<CollectionBulkOrderDeed> LoadFromXml(XmlNode rootNode)
        {
            var retVal = new List<CollectionBulkOrderDeed>();
            var nodes = rootNode.SelectNodes($"{XmlRootName}/{_XmlItemName}");

            if (nodes != null)
            {
                foreach (var node in nodes.OfType<XmlNode>())
                {
                    try
                    {
                        var idString = node.Attributes[_IdAttributeName].Value;
                        var profession = node.Attributes[_ProfessionAttributeName].Value;
                        var name = node.Attributes[_NameAttributeName].Value;
                        var bulkOrderDeedTypeString = node.Attributes[_TypeAttributeName].Value;
                        var quantityString = node.Attributes[_QuantityAttributeName].Value;
                        var exceptionalString = node.Attributes[_ExceptionalAttributeName].Value;
                        var material = node.Attributes[_MaterialAttributeName].Value;
                        var vendorString = node.Attributes[_VendorAttributeName].Value;
                        var bulkOrderDeedBookString = node.Attributes[_BulkOrderDeedBookAttributeName].Value;

                        if (!String.IsNullOrEmpty(idString) && !String.IsNullOrEmpty(name))
                        {
                            var id = Guid.Parse(idString);
                            var bulkOrderDeedType = (BulkOrderDeedType)Enum.Parse(typeof(BulkOrderDeedType), bulkOrderDeedTypeString);
                            var quantity = int.Parse(quantityString);
                            var exceptional = bool.Parse(exceptionalString);
                            var vendor = Guid.Parse(vendorString);
                            var bulkOrderDeedBook = Guid.Parse(bulkOrderDeedBookString);

                            if (bulkOrderDeedType == BulkOrderDeedType.Small)
                            {
                                retVal.Add(SmallCollectionBulkOrderDeed.LoadFromXml(node, id, profession, name, quantity, exceptional, material, vendor, bulkOrderDeedBook));
                            }
                            else
                            {
                                retVal.Add(LargeCollectionBulkOrderDeed.LoadFromXml(node, id, profession, name, quantity, exceptional, material, vendor, bulkOrderDeedBook));
                            }
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
            string quality = Exceptional ? "Exceptional" : "Normal";
            string material = String.IsNullOrEmpty(Material) ? " " : $" {Material}";

            return $"{Quantity} {quality}{material} ({Profession})";
        }
    }
}
