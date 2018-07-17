using Npe.UO.BulkOrderDeeds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class AddBulkOrderDeedToCollectionViewModel : ViewModelBase
    {
        private CollectionBulkOrderDeed _CollectionBulkOrderDeed;

        public string Profession
        {
            get { return _CollectionBulkOrderDeed.Profession; }
        }

        public int Quantity
        {
            get { return _CollectionBulkOrderDeed.Quantity; }
        }

        public bool Exceptional
        {
            get { return _CollectionBulkOrderDeed.Exceptional; }
        }

        public string Material
        {
            get { return _CollectionBulkOrderDeed.Material; }
        }

        private string _NewVendor;
        public string NewVendor
        {
            get { return _NewVendor; }
            set
            {
                if (_NewVendor == value) return;

                _NewVendor = value;
                SetBulkOrderDeedBooks();
                NotifyPropertyChanged(nameof(NewVendor));
            }
        }

        private string _NewBulkOrderDeedBook;
        public string NewBulkOrderDeedBook
        {
            get { return _NewBulkOrderDeedBook; }
            set
            {
                if (_NewBulkOrderDeedBook == value) return;

                _NewBulkOrderDeedBook = value;
                NotifyPropertyChanged(nameof(NewBulkOrderDeedBook));
            }
        }

        private VendorViewModel _SelectedVendor;
        public VendorViewModel SelectedVendor
        {
            get { return _SelectedVendor; }
            set
            {
                if (_SelectedVendor == value) return;

                _SelectedVendor = value;
                SetBulkOrderDeedBooks();
                _CollectionBulkOrderDeed.Location.Vendor = _SelectedVendor != null ? _SelectedVendor.Vendor : Vendor.None;
                NotifyPropertyChanged(nameof(SelectedVendor));
            }
        }

        private BulkOrderDeedBookViewModel _SelectedBulkOrderDeedBook;
        public BulkOrderDeedBookViewModel SelectedBulkOrderDeedBook
        {
            get { return _SelectedBulkOrderDeedBook; }
            set
            {
                if (_SelectedBulkOrderDeedBook == value) return;

                _SelectedBulkOrderDeedBook = value;
                _CollectionBulkOrderDeed.Location.BulkOrderDeedBook = _SelectedBulkOrderDeedBook != null ?_SelectedBulkOrderDeedBook.BulkOrderDeedBook : BulkOrderDeedBook.None;
                NotifyPropertyChanged(nameof(SelectedBulkOrderDeedBook));
            }
        }

        private ObservableCollection<VendorViewModel> _Vendors;
        public ObservableCollection<VendorViewModel> Vendors
        {
            get { return _Vendors; }
            set
            {
                _Vendors = value;
                NotifyPropertyChanged(nameof(Vendors));
            }
        }

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

        public ICommand AddToCollectionCommand { get; }
        public ICommand CancelCommand { get; }

        public AddBulkOrderDeedToCollectionViewModel(CollectionBulkOrderDeed collectionBulkOrderDeed)
        {
            BackCommandVisibility = System.Windows.Visibility.Visible;
            HomeCommandVisibility = System.Windows.Visibility.Visible;
            AddToCollectionCommand = new RelayCommand(OnAddToCollectionCommand);
            CancelCommand = new RelayCommand(OnCancelCommand);

            _CollectionBulkOrderDeed = collectionBulkOrderDeed;

            var bulkOrderDeedItems = new List<CollectionBulkOrderDeedItemViewModel>();

            foreach (var collectionBulkOrderDeedItem in collectionBulkOrderDeed.CollectionBulkOrderDeedItems)
            {
                bulkOrderDeedItems.Add(new CollectionBulkOrderDeedItemViewModel(collectionBulkOrderDeedItem));
            }

            _BulkOrderDeedItems = new ObservableCollection<CollectionBulkOrderDeedItemViewModel>(bulkOrderDeedItems);

            var vendors = new List<VendorViewModel>();

            vendors.Add(VendorViewModel.None);

            foreach (var vendor in BulkOrderDeedManager.Instance.Vendors)
            {
                vendors.Add(new VendorViewModel(vendor));
            }

            _Vendors = new ObservableCollection<VendorViewModel>(vendors);

            var bulkOrderDeedBooks = new List<BulkOrderDeedBookViewModel>();

            bulkOrderDeedBooks.Add(BulkOrderDeedBookViewModel.None);

            foreach (var bulkOrderDeedBook in BulkOrderDeedManager.Instance.BulkOrderDeedBooks)
            {
                bulkOrderDeedBooks.Add(new BulkOrderDeedBookViewModel(bulkOrderDeedBook));
            }

            _BulkOrderDeedBooks = new ObservableCollection<BulkOrderDeedBookViewModel>(bulkOrderDeedBooks);

            _SelectedVendor = VendorViewModel.None;
            _SelectedBulkOrderDeedBook = BulkOrderDeedBookViewModel.None;
        }

        private void SetBulkOrderDeedBooks()
        {
            BulkOrderDeedBooks.Clear();
            BulkOrderDeedBooks.Add(BulkOrderDeedBookViewModel.None);
            SelectedBulkOrderDeedBook = BulkOrderDeedBookViewModel.None;

            if (_SelectedVendor == null || _SelectedVendor == VendorViewModel.None)
            {
                if (String.IsNullOrEmpty(_NewVendor) || _NewVendor == Vendor.None.Name)
                {
                    foreach (var bulkOrderDeedBook in BulkOrderDeedManager.Instance.BulkOrderDeedBooks)
                    {
                        BulkOrderDeedBooks.Add(new BulkOrderDeedBookViewModel(bulkOrderDeedBook));
                    }
                }
            }
            else
            {
                foreach (var bulkOrderDeedBook in _SelectedVendor.BulkOrderDeedBooks)
                {
                    BulkOrderDeedBooks.Add(new BulkOrderDeedBookViewModel(bulkOrderDeedBook.BulkOrderDeedBook));
                }
            }
        }

        private void OnAddToCollectionCommand(object parameter)
        {
            var bulkOrderDeedBook = BulkOrderDeedBook.None;
            var isNewBook = false;

            if (_SelectedBulkOrderDeedBook == null || _SelectedBulkOrderDeedBook == BulkOrderDeedBookViewModel.None)
            {
                if (!String.IsNullOrEmpty(_NewBulkOrderDeedBook) && _NewBulkOrderDeedBook != BulkOrderDeedBook.None.Name)
                {
                    bulkOrderDeedBook = new BulkOrderDeedBook(_NewBulkOrderDeedBook);
                    isNewBook = true;
                }
            }
            else
            {
                bulkOrderDeedBook = _SelectedBulkOrderDeedBook.BulkOrderDeedBook;
            }

            if (_SelectedVendor == null || _SelectedVendor == VendorViewModel.None)
            {
                if (!String.IsNullOrEmpty(_NewVendor) && _NewVendor != VendorViewModel.None.Name)
                {
                    var vendor = new Vendor(_NewVendor);

                    if (isNewBook)
                    {
                        vendor.AddBulkOrderDeedBook(bulkOrderDeedBook);
                    }

                    BulkOrderDeedManager.Instance.AddVendor(vendor);
                    _CollectionBulkOrderDeed.Location.Vendor = vendor;

                    if (isNewBook)
                    {
                        _CollectionBulkOrderDeed.Location.BulkOrderDeedBook = bulkOrderDeedBook;
                    }
                }
                else
                {
                    if (isNewBook)
                    {
                        BulkOrderDeedManager.Instance.AddBulkOrderDeedBook(bulkOrderDeedBook);
                        _CollectionBulkOrderDeed.Location.BulkOrderDeedBook = bulkOrderDeedBook;
                    }
                }
            }
            else
            {
                if (isNewBook)
                {
                    _SelectedVendor.Vendor.AddBulkOrderDeedBook(bulkOrderDeedBook);
                    _CollectionBulkOrderDeed.Location.BulkOrderDeedBook = bulkOrderDeedBook;
                }
            }

            BulkOrderDeedManager.Instance.AddBulkOrderDeeds(new[] { _CollectionBulkOrderDeed });
            NavigationController.Instance.Navigate(NavigateTypes.Back);
        }

        private void OnCancelCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.Back);
        }
    }
}
