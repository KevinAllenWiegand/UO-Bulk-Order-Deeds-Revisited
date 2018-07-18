using System;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class SmallCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        private const string _CompletedCountAttributeName = "completedCount";

        public override BulkOrderDeedType BulkOrderDeedType => BulkOrderDeedType.Small;
        public int CompletedCount { get; set; }

        public SmallCollectionBulkOrderDeed(Profession profession, SmallBulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, Vendor.None, BulkOrderDeedBook.None)
        {
        }

        public SmallCollectionBulkOrderDeed(Profession profession, SmallBulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material, Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook, int completedCount)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, vendor, bulkOrderDeedBook)
        {
            CompletedCount = completedCount;
        }

        public SmallCollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook, int completedCount)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Small, quantity, exceptional, material, vendor, bulkOrderDeedBook)
        {
            CompletedCount = completedCount;
        }

        internal static CollectionBulkOrderDeed LoadFromXml(XmlNode node, Guid id, string profession, string name, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook)
        {
            var completedCountString = node.Attributes[_CompletedCountAttributeName].Value;
            int completedCount = int.Parse(completedCountString);

            return new SmallCollectionBulkOrderDeed(id, profession, name, quantity, exceptional, material, vendor, bulkOrderDeedBook, completedCount);
        }

        protected override void SaveToXmlImpl(XmlWriter writer)
        {
            writer.WriteAttributeString(_CompletedCountAttributeName, CompletedCount.ToString());
        }
    }
}
