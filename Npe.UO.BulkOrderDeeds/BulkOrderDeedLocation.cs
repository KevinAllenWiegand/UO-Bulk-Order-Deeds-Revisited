using System;
using System.Text;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedLocation
    {
        public Vendor Vendor { get; set; }
        public BulkOrderDeedBook BulkOrderDeedBook { get; set; }

        public BulkOrderDeedLocation(Vendor vendor, BulkOrderDeedBook bulkOrderDeedBook)
        {
            if (vendor == null && bulkOrderDeedBook == null)
            {
                throw new ArgumentNullException($"At least one parameter cannot be null:  vendor, bulkOrderDeedBook");
            }

            Vendor = vendor;
            BulkOrderDeedBook = bulkOrderDeedBook;
        }

        public override string ToString()
        {
            var retVal = new StringBuilder();

            if (Vendor != null)
            {
                retVal.Append(Vendor);
            }

            if (BulkOrderDeedBook != null)
            {
                if (retVal.Length > 0)
                {
                    retVal.Append("; ");
                }

                retVal.Append(BulkOrderDeedBook);
            }

            return retVal.ToString();
        }
    }
}
