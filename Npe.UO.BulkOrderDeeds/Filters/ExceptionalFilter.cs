namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class ExceptionalFilter : IBulkOrderDeedFilter
    {
        public bool? Value { get; }

        public ExceptionalFilter()
            : this(null)
        {
        }

        public ExceptionalFilter(bool? value)
        {
            Value = value;
        }

        public bool ApplyFilter(CollectionBulkOrderDeed bulkOrderDeed)
        {
            if (Value == null || !Value.HasValue) return true;

            return bulkOrderDeed.Exceptional == Value.Value;
        }
    }
}
