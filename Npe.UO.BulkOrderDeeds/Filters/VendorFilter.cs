namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class VendorFilter : IBulkOrderDeedFilter
    {
        public Vendor Value { get; }

        public VendorFilter()
            : this(null)
        {
        }

        public VendorFilter(Vendor value)
        {
            Value = value;
        }

        public bool ApplyFilter(CollectionBulkOrderDeed bulkOrderDeed)
        {
            // TODO
            return true;
        }
    }
}
