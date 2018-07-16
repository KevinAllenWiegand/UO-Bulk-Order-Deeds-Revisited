
using System;

namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class BulkOrderDeedNameFilter : IBulkOrderDeedFilter
    {
        public string Value { get; }

        public BulkOrderDeedNameFilter()
            : this(null)
        {
        }

        public BulkOrderDeedNameFilter(string value)
        {
            Value = value;
        }

        public bool ApplyFilter(CollectionBulkOrderDeed bulkOrderDeed)
        {
            if (String.IsNullOrEmpty(Value) || String.IsNullOrWhiteSpace(Value)) return true;

            return bulkOrderDeed.DisplayName.ToLower().Contains(Value.ToLower());
        }
    }
}
