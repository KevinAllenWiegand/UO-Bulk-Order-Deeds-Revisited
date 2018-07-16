namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class BulkOrderDeedTypeFilter : IBulkOrderDeedFilter
    {
        public BulkOrderDeedType? Value { get; }

        public BulkOrderDeedTypeFilter()
            : this(null)
        {
        }

        public BulkOrderDeedTypeFilter(BulkOrderDeedType? value)
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
