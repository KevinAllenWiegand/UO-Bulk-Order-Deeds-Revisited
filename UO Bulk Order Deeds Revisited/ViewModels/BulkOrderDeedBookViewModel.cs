using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Internal;
using System;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedBookViewModel : ViewModelBase
    {
        public static BulkOrderDeedBookViewModel None = new BulkOrderDeedBookViewModel(BulkOrderDeedBook.None);

        public Guid Id => BulkOrderDeedBook.Id;
        public string Name => BulkOrderDeedBook.Name;
        public BulkOrderDeedBook BulkOrderDeedBook { get; }

        public BulkOrderDeedBookViewModel(BulkOrderDeedBook bulkOrderDeedBook)
        {
            Guard.ArgumentNotNull(nameof(bulkOrderDeedBook), bulkOrderDeedBook);

            BulkOrderDeedBook = bulkOrderDeedBook;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
