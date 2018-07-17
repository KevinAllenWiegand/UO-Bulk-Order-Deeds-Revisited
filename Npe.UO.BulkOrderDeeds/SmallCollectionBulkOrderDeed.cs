using System;
using System.Collections.Generic;
using System.Linq;

namespace Npe.UO.BulkOrderDeeds
{
    public class SmallCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public override BulkOrderDeedType BulkOrderDeedType => BulkOrderDeedType.Small;

        public SmallCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, null, null)
        {
        }

        public SmallCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material, Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook, int completedCount)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, vendor, bulkOrderDeedBook)
        {
            var item = CollectionBulkOrderDeedItems.First();
            var items = new Dictionary<SmallBulkOrderDeedDefinition, int>();

            items[(SmallBulkOrderDeedDefinition)bulkOrderDeedDefinition] = completedCount;

            SetCompletedCounts(items);
        }

        public SmallCollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook, IEnumerable<CollectionBulkOrderDeedItem> bulkOrderDeedItems)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Small, quantity, exceptional, material, vendor, bulkOrderDeedBook, bulkOrderDeedItems)
        {
        }
    }
}
