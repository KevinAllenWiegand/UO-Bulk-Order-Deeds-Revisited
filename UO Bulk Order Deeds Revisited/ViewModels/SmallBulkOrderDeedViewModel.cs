using Npe.UO.BulkOrderDeeds;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class SmallBulkOrderDeedViewModel : BulkOrderDeedViewModel
    {
        private SmallCollectionBulkOrderDeed _SmallCollectionBulkOrderDeed;

        public int CompletedCount
        {
            get { return _SmallCollectionBulkOrderDeed.CompletedCount; }
            set
            {
                _SmallCollectionBulkOrderDeed.CompletedCount = value;
                NotifyPropertyChanged(nameof(CompletedCount));
            }
        }

        public SmallBulkOrderDeedViewModel(SmallCollectionBulkOrderDeed smallCollectionBulkOrderDeed)
            : base(smallCollectionBulkOrderDeed)
        {
            _SmallCollectionBulkOrderDeed = smallCollectionBulkOrderDeed;
        }
    }
}
