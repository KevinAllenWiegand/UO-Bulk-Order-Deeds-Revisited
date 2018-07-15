using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class VendorViewModel : ViewModelBase
    {
        public static VendorViewModel None = new VendorViewModel(Vendor.None);

        public Guid Id => Vendor.Id;
        public string Name => Vendor.Name;
        public Vendor Vendor { get; }

        private ObservableCollection<BulkOrderDeedBookViewModel> _BulkOrderDeedBooks;
        public ObservableCollection<BulkOrderDeedBookViewModel> BulkOrderDeedBooks
        {
            get { return _BulkOrderDeedBooks; }
            set
            {
                _BulkOrderDeedBooks = value;
                NotifyPropertyChanged(nameof(BulkOrderDeedBooks));
            }
        }

        public VendorViewModel(Vendor vendor)
        {
            Guard.ArgumentNotNull(nameof(vendor), vendor);

            Vendor = vendor;

            var bulkOrderDeedBooks = new List<BulkOrderDeedBookViewModel>();

            foreach (var bulkOrderDeedBook in Vendor.BulkOrderDeedBooks)
            {
                bulkOrderDeedBooks.Add(new BulkOrderDeedBookViewModel(bulkOrderDeedBook));
            }

            _BulkOrderDeedBooks = new ObservableCollection<BulkOrderDeedBookViewModel>(bulkOrderDeedBooks);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
