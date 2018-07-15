using System;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds
{
    public class SmallCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public override BulkOrderDeedType BulkOrderDeedType => BulkOrderDeedType.Small;

        public SmallCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material)
        {
        }

        public SmallCollectionBulkOrderDeed(Guid id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material, IEnumerable<CollectionBulkOrderDeedItem> bulkOrderDeedItems)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Small, quantity, exceptional, material, bulkOrderDeedItems)
        {
        }
    }
}
