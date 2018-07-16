using System;

namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class BulkOrderDeedMaterialFilter : IBulkOrderDeedFilter
    {
        public BulkOrderDeedMaterial Value { get; }

        public BulkOrderDeedMaterialFilter()
            : this(null)
        {
        }

        public BulkOrderDeedMaterialFilter(BulkOrderDeedMaterial value)
        {
            Value = value;
        }

        public bool ApplyFilter(CollectionBulkOrderDeed bulkOrderDeed)
        {
            if (Value == null) return true;

            return String.Compare(bulkOrderDeed.Material, Value.Name, true) == 0;
        }
    }
}
