using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedEventArgs : EventArgs
    {
        private List<CollectionBulkOrderDeed> _BulkOrderDeeds;

        public IEnumerable<CollectionBulkOrderDeed> BulkOrderDeeds => _BulkOrderDeeds.AsReadOnly();

        public BulkOrderDeedEventArgs(IEnumerable<CollectionBulkOrderDeed> bulkOrderDeeds)
        {
            Guard.ArgumentCollectionNotNullOrEmpty(nameof(bulkOrderDeeds), bulkOrderDeeds);

            _BulkOrderDeeds = new List<CollectionBulkOrderDeed>(bulkOrderDeeds);
        }
    }
}
