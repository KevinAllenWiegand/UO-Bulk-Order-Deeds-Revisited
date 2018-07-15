using System;

namespace Npe.UO.BulkOrderDeeds
{
    public class LargeCollectionBulkOrderDeed : CollectionBulkOrderDeed
    {
        public LargeCollectionBulkOrderDeed(Profession profession, BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool exceptional, BulkOrderDeedMaterial material)
            : base(profession, bulkOrderDeedDefinition, quantity, exceptional, material)
        {
        }

        public LargeCollectionBulkOrderDeed(string id, string profession, string bulkOrderDeedNameMatch, int quantity, bool exceptional, string material)
            : base(id, profession, bulkOrderDeedNameMatch, BulkOrderDeedType.Large, quantity, exceptional, material)
        {
        }
    }
}
