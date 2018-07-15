using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds.Internal
{
    internal class BulkOrderDeedPointEntryComparer : IComparer<BulkOrderDeedPointEntry>
    {
        public int Compare(BulkOrderDeedPointEntry x, BulkOrderDeedPointEntry y)
        {
            if (x == null && y == null) return 0;
            if (x == null && y != null) return -1;
            if (x != null && y == null) return 1;

            return x.PointDifference.CompareTo(y.PointDifference);
        }
    }
}
