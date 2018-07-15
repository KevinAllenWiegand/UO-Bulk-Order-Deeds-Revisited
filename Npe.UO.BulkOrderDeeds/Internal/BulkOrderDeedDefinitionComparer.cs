using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds.Internal
{
    internal class BulkOrderDeedDefinitionComparer : IComparer<BulkOrderDeedDefinition>
    {
        public int Compare(BulkOrderDeedDefinition x, BulkOrderDeedDefinition y)
        {
            if (x == null && y == null) return 0;
            if (x == null && y != null) return -1;
            if (x != null && y == null) return 1;

            // Compare on Name if both are small bulk order deeds.
            if ((x is SmallBulkOrderDeedDefinition smallX1) && y is SmallBulkOrderDeedDefinition smallY1)
            {
                return smallX1.Name.CompareTo(smallY1.Name);
            }

            // Compare on BulkOrderDeedType if both are small bulk order deeds.
            if ((x is LargeBulkOrderDeedDefinition largeX1) && y is LargeBulkOrderDeedDefinition largeY1)
            {
                return largeX1.BulkOrderDeedType.CompareTo(largeY1.BulkOrderDeedType);
            }

            // Small bulk order deeds come first.
            if ((x is SmallBulkOrderDeedDefinition smallX2) && y is LargeBulkOrderDeedDefinition largeY2)
            {
                return -1;
            }

            // Small bulk order deeds come first.
            if ((x is LargeBulkOrderDeedDefinition largeX2) && y is SmallBulkOrderDeedDefinition smallY2)
            {
                return 1;
            }

            return 0;
        }
    }
}
