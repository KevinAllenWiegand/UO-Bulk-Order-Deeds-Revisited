using Npe.UO.BulkOrderDeeds;
using System;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedViewModel : ViewModelBase
    {
        public CollectionBulkOrderDeed CollectionBulkOrderDeed { get; }

        public string Profession => CollectionBulkOrderDeed.Profession;
        public string DisplayName => CollectionBulkOrderDeed.DisplayName;
        public int Quantity => CollectionBulkOrderDeed.Quantity;
        public bool IsExceptional => CollectionBulkOrderDeed.Exceptional;
        public string Material => CollectionBulkOrderDeed.Material;
        public string Vendor => CollectionBulkOrderDeed.Location.Vendor != Npe.UO.BulkOrderDeeds.Vendor.None ? CollectionBulkOrderDeed.Location.Vendor.ToString() : String.Empty;
        public string BulkOrderDeedBook => CollectionBulkOrderDeed.Location.BulkOrderDeedBook != Npe.UO.BulkOrderDeeds.BulkOrderDeedBook.None ? CollectionBulkOrderDeed.Location.BulkOrderDeedBook.ToString() : String.Empty;

        public BulkOrderDeedViewModel(CollectionBulkOrderDeed collectionBulkOrderDeed)
        {
            CollectionBulkOrderDeed = collectionBulkOrderDeed;
        }
    }
}
