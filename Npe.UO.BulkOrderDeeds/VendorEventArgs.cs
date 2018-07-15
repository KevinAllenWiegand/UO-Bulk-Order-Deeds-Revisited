using Npe.UO.BulkOrderDeeds.Internal;
using System;

namespace Npe.UO.BulkOrderDeeds
{
    public class VendorEventArgs : EventArgs
    {
        public Vendor Vendor { get; }

        public VendorEventArgs(Vendor vendor)
        {
            Guard.ArgumentNotNull(nameof(vendor), vendor);

            Vendor = vendor;
        }
    }
}
