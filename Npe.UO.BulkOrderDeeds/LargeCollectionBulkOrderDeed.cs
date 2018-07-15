using System;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds
{
    public class LargeCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public override BulkOrderDeedType BulkOrderDeedType => BulkOrderDeedType.Large;

        public LargeCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material)
        {
        }

        public LargeCollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material, IEnumerable<CollectionBulkOrderDeedItem> bulkOrderDeedItems)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Large, quantity, exceptional, material, bulkOrderDeedItems)
        {
        }
    }
}
