using Npe.UO.BulkOrderDeeds.Internal;
using System;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedBookEventArgs : EventArgs
    {
        public Vendor Vendor { get; }
        public BulkOrderDeedBook BulkOrderDeedBook { get; }

        public BulkOrderDeedBookEventArgs(Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook)
        {
            // Vendor can be null.
            Guard.ArgumentNotNull(nameof(bulkOrderDeedBook), bulkOrderDeedBook);

            BulkOrderDeedBook = bulkOrderDeedBook;
            Vendor = vendor;
        }

        public BulkOrderDeedBookEventArgs(BulkOrderDeedBook bulkOrderDeedBook)
            : this(null, bulkOrderDeedBook)
        {
        }
    }
}
