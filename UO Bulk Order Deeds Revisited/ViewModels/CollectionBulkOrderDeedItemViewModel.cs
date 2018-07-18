using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class CollectionBulkOrderDeedItemViewModel : ViewModelBase
    {
        private CollectionBulkOrderDeedItem _CollectionBulkOrderDeedItem;

        public string Name => _CollectionBulkOrderDeedItem.Name;

        public bool IsCompleted
        {
            get { return _CollectionBulkOrderDeedItem.IsCompleted; }
            set
            {
                if (_CollectionBulkOrderDeedItem.IsCompleted == value) return;

                _CollectionBulkOrderDeedItem.IsCompleted = value;
                NotifyPropertyChanged(nameof(IsCompleted));
                NotifyPropertyChanged(nameof(CompletedCount));
            }
        }

        public int Quantity => _CollectionBulkOrderDeedItem.Quantity;
        public int CompletedCount => IsCompleted ? Quantity : 0;

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
