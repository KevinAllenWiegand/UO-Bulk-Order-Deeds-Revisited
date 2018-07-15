using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class CollectionBulkOrderDeedItemViewModel : ViewModelBase
    {
        private CollectionBulkOrderDeedItem _CollectionBulkOrderDeedItem;

        public string Name => _CollectionBulkOrderDeedItem.Name;

        public int CompletedCount
        {
            get { return _CollectionBulkOrderDeedItem.CompletedCount; }
            set
            {
                if (_CollectionBulkOrderDeedItem.CompletedCount == value) return;

                _CollectionBulkOrderDeedItem.CompletedCount = value;
                NotifyPropertyChanged(nameof(CompletedCount));
            }
        }

        public int Quantity => _CollectionBulkOrderDeedItem.Quantity;

        public CollectionBulkOrderDeedItemViewModel(CollectionBulkOrderDeedItem collectionBulkOrderDeedItem)
        {
            Guard.ArgumentNotNull(nameof(collectionBulkOrderDeedItem), collectionBulkOrderDeedItem);

            _CollectionBulkOrderDeedItem = collectionBulkOrderDeedItem;
        }

        public override string ToString()
        {
            return _CollectionBulkOrderDeedItem.ToString();
        }
    }
}
