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
            if (Value == null) return true;

            return bulkOrderDeed.Location?.Vendor.Id == Value.Id;
        }
    }
}
