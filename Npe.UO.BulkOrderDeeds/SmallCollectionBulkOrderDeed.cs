using System;

namespace Npe.UO.BulkOrderDeeds
{
    public class SmallCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public SmallCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material)
        {
        }

        public SmallCollectionBulkOrderDeed(string id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Small, quantity, exceptional, material)
        {
        }
    }
}
