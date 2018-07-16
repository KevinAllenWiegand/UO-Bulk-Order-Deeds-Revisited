namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class ProfessionFilter : IBulkOrderDeedFilter
    {
        public Profession Value { get; }

        public ProfessionFilter()
            : this(null)
        {
        }

        public ProfessionFilter(Profession value)
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
