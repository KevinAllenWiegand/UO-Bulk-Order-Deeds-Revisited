namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class BooleanFilter : IBulkOrderDeedFilter
    {
        public bool? Value { get; }

        public BooleanFilter()
            : this(null)
        {
        }

        public BooleanFilter(bool? value)
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
