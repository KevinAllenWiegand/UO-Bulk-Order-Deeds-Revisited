namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class BulkOrderDeedBookFilter : IBulkOrderDeedFilter
    {
        public BulkOrderDeedBook Value { get; }

        public BulkOrderDeedBookFilter()
            : this(null)
        {
        }

        public BulkOrderDeedBookFilter(BulkOrderDeedBook value)
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
