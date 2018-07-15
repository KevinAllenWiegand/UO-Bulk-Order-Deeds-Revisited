using System.Collections.Generic;

namespace ConvertOldBulkOrderDeedDataFiles
{
    internal class OldLargeBulkOrderDeed
    {
        private readonly OldLargeBulkOrderDeedMapItem _MapItem;

        public string Name => _MapItem.BulkOrderDeedName;
        public IEnumerable<string> Categories => _MapItem.Categories;
        public IEnumerable<string> SmallBulkOrderDeedItemNames { get; }

        public OldLargeBulkOrderDeed(OldLargeBulkOrderDeedMapItem mapItem, List<string> itemNames)
        {
            _MapItem = mapItem;
            SmallBulkOrderDeedItemNames = new List<string>(itemNames);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
