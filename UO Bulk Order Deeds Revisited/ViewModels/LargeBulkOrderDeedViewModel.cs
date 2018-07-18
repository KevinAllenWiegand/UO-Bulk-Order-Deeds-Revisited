using Npe.UO.BulkOrderDeeds;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class LargeBulkOrderDeedViewModel : BulkOrderDeedViewModel
    {
        private LargeCollectionBulkOrderDeed _LargeCollectionBulkOrderDeed;

        private ObservableCollection<CollectionBulkOrderDeedItemViewModel> _BulkOrderDeedItems;
        public ObservableCollection<CollectionBulkOrderDeedItemViewModel> BulkOrderDeedItems
        {
            get { return _BulkOrderDeedItems; }
            set
            {
                _BulkOrderDeedItems = value;
                NotifyPropertyChanged(nameof(BulkOrderDeedItems));
            }
        }

        public LargeBulkOrderDeedViewModel(LargeCollectionBulkOrderDeed largeCollectionBulkOrderDeed)
            : base(largeCollectionBulkOrderDeed)
        {
            _LargeCollectionBulkOrderDeed = largeCollectionBulkOrderDeed;

            var bulkOrderDeedItems = new List<CollectionBulkOrderDeedItemViewModel>();

            foreach (var bulkOrderDeedItem in largeCollectionBulkOrderDeed.CollectionBulkOrderDeedItems)
            {
                bulkOrderDeedItems.Add(new CollectionBulkOrderDeedItemViewModel(bulkOrderDeedItem));
            }

            _BulkOrderDeedItems = new ObservableCollection<CollectionBulkOrderDeedItemViewModel>(bulkOrderDeedItems);
        }
    }
}
