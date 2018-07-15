using Npe.UO.BulkOrderDeeds;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class CollectionViewModel : ViewModelBase
    {
        private ObservableCollection<BulkOrderDeedViewModel> _BulkOrderDeeds;
        public ObservableCollection<BulkOrderDeedViewModel> BulkOrderDeeds
        {
            get { return _BulkOrderDeeds; }
            set
            {
                _BulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(BulkOrderDeeds));
            }
        }

        public CollectionViewModel()
        {
            BackCommandVisibility = System.Windows.Visibility.Visible;
            CollectionCommandVisibility = System.Windows.Visibility.Collapsed;

            var bulkOrderDeeds = new List<BulkOrderDeedViewModel>();

            foreach (var bulkOrderDeed in BulkOrderDeedManager.Instance.Collection)
            {
                bulkOrderDeeds.Add(new BulkOrderDeedViewModel(bulkOrderDeed));
            }

            _BulkOrderDeeds = new ObservableCollection<BulkOrderDeedViewModel>(bulkOrderDeeds);

            BulkOrderDeedManager.Instance.BulkOrderDeedCollectionItemAdded += BulkOrderDeedCollectionItemAdded;
            BulkOrderDeedManager.Instance.BulkOrderDeedCollectionItemRemoved += BulkOrderDeedCollectionItemRemoved;
        }

        private void BulkOrderDeedCollectionItemAdded(object sender, BulkOrderDeedEventArgs e)
        {
            _BulkOrderDeeds.Add(new BulkOrderDeedViewModel(e.BulkOrderDeed));
        }

        private void BulkOrderDeedCollectionItemRemoved(object sender, BulkOrderDeedEventArgs e)
        {
            var bulkOrderDeedViewModel = _BulkOrderDeeds.FirstOrDefault(b => b.CollectionBulkOrderDeed == e.BulkOrderDeed);

            _BulkOrderDeeds.Remove(bulkOrderDeedViewModel);
        }
    }
}
