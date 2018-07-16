using System;

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
            if (Value == null) return true;

            return String.Compare(Value.Name, bulkOrderDeed.Profession, true) == 0;
        }
    }
}
