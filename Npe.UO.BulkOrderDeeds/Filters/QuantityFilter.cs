namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class QuantityFilter : IBulkOrderDeedFilter
    {
        public int? Value { get; }

        public QuantityFilter()
            : this(null)
        {
        }

        public QuantityFilter(int? value)
        {
            Value = value;
        }

        public bool ApplyFilter(CollectionBulkOrderDeed bulkOrderDeed)
        {
            if (Value == null || !Value.HasValue) return true;

            return bulkOrderDeed.Quantity == Value.Value;
        }
    }
}
