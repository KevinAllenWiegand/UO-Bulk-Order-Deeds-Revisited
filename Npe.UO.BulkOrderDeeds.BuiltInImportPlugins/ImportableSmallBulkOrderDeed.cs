using System;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public class ImportableSmallBulkOrderDeed : ImportableBulkOrderDeed
    {
        public int CompletedCount { get; }

        public ImportableSmallBulkOrderDeed(Guid id, string profession, bool exceptional, string material, string name, int quantity, int completedCount, Guid bodBook)
            : base(id, profession, exceptional, material, name, quantity, bodBook)
        {
            CompletedCount = completedCount;
        }
    }
}
