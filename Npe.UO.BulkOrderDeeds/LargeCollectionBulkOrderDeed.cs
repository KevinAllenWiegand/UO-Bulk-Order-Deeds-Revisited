using System;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds
{
    public class LargeCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public override BulkOrderDeedType BulkOrderDeedType => BulkOrderDeedType.Large;

        public LargeCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, null, null)
        {
        }

        public LargeCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material, Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook, IDictionary<SmallBulkOrderDeedDefinition, int> completedCounts)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material, vendor, bulkOrderDeedBook)
        {
            SetCompletedCounts(completedCounts);
        }

        public LargeCollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material, Guid vendor, Guid bulkOrderDeedBook, IEnumerable<CollectionBulkOrderDeedItem> bulkOrderDeedItems)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Large, quantity, exceptional, material, vendor, bulkOrderDeedBook, bulkOrderDeedItems)
        {
        }
    }
}
