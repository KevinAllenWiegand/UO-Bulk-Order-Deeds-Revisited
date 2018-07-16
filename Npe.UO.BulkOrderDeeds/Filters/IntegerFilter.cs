namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class IntegerFilter : IBulkOrderDeedFilter
    {
        public int? Value { get; }

        public IntegerFilter()
            : this(null)
        {
        }

        public IntegerFilter(int? value)
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
