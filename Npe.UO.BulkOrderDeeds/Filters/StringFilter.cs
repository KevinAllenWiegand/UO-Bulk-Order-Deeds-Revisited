namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class StringFilter : IBulkOrderDeedFilter
    {
        public string Value { get; }

        public StringFilter()
            : this(null)
        {
        }

        public StringFilter(string value)
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
