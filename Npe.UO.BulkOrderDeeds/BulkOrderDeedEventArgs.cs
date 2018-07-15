using Npe.UO.BulkOrderDeeds.Internal;
using System;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedEventArgs : EventArgs
    {
        public CollectionBulkOrderDeed BulkOrderDeed { get; }

        public BulkOrderDeedEventArgs(CollectionBulkOrderDeed bulkOrderDeed)
        {
            Guard.ArgumentNotNull(nameof(bulkOrderDeed), bulkOrderDeed);

            BulkOrderDeed = bulkOrderDeed;
        }
    }
}
