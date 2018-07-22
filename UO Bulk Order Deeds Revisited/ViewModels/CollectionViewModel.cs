using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class CollectionViewModel : ViewModelBase
    {
        private CollectionFilterParameters _CollectionFilterParameters;
        private List<BulkOrderDeedViewModel> _AllBulkOrderDeeds;
        private const int _PageSize = 25;
        private bool _NeedsRefresh = true;

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

        public ProfessionFilterViewModel _SelectedProfessionFilter = ProfessionFilterViewModel.None;
        public ProfessionFilterViewModel SelectedProfessionFilter
        {
            get { return _SelectedProfessionFilter; }
            set
            {
                _SelectedProfessionFilter = value;
                _CollectionFilterParameters.Profession = new ProfessionFilter(value.Value);
                SetupMaterialFilters();
                NotifyPropertyChanged(nameof(SelectedProfessionFilter));
            }
        }

        public ObservableCollection<ProfessionFilterViewModel> _ProfessionFilters;
        public ObservableCollection<ProfessionFilterViewModel> ProfessionFilters
        {
            get { return _ProfessionFilters; }
            set
            {
                _ProfessionFilters = value;
                NotifyPropertyChanged(nameof(ProfessionFilters));
            }
        }

        public BulkOrderDeedTypeFilterViewModel _SelectedBulkOrderDeedTypeFilter = BulkOrderDeedTypeFilterViewModel.None;
        public BulkOrderDeedTypeFilterViewModel SelectedBulkOrderDeedTypeFilter
        {
            get { return _SelectedBulkOrderDeedTypeFilter; }
            set
            {
                _SelectedBulkOrderDeedTypeFilter = value;
                _CollectionFilterParameters.BulkOrderDeedType = new BulkOrderDeedTypeFilter(value.Value);
                NotifyPropertyChanged(nameof(SelectedBulkOrderDeedTypeFilter));
            }
        }

        public ObservableCollection<BulkOrderDeedTypeFilterViewModel> _BulkOrderDeedTypeFilters;
        public ObservableCollection<BulkOrderDeedTypeFilterViewModel> BulkOrderDeedTypeFilters
        {
            get { return _BulkOrderDeedTypeFilters; }
            set
            {
                _BulkOrderDeedTypeFilters = value;
                NotifyPropertyChanged(nameof(BulkOrderDeedTypeFilters));
            }
        }

        public ExceptionalFilterViewModel _SelectedExceptionalFilter = ExceptionalFilterViewModel.None;
        public ExceptionalFilterViewModel SelectedExceptionalFilter
        {
            get { return _SelectedExceptionalFilter; }
            set
            {
                _SelectedExceptionalFilter = value;
                _CollectionFilterParameters.Exceptional = new ExceptionalFilter(value.Value);
                NotifyPropertyChanged(nameof(SelectedExceptionalFilter));
            }
        }

        public ObservableCollection<ExceptionalFilterViewModel> _ExceptionalFilters;
        public ObservableCollection<ExceptionalFilterViewModel> ExceptionalFilters
        {
            get { return _ExceptionalFilters; }
            set
            {
                _ExceptionalFilters = value;
                NotifyPropertyChanged(nameof(ExceptionalFilters));
            }
        }

        public QuantityFilterViewModel _SelectedQuantityFilter = QuantityFilterViewModel.None;
        public QuantityFilterViewModel SelectedQuantityFilter
        {
            get { return _SelectedQuantityFilter; }
            set
            {
                _SelectedQuantityFilter = value;
                _CollectionFilterParameters.Quantity = new QuantityFilter(value.Value);
                NotifyPropertyChanged(nameof(SelectedQuantityFilter));
            }
        }

        public ObservableCollection<QuantityFilterViewModel> _QuantityFilters;
        public ObservableCollection<QuantityFilterViewModel> QuantityFilters
        {
            get { return _QuantityFilters; }
            set
            {
                _QuantityFilters = value;
                NotifyPropertyChanged(nameof(QuantityFilters));
            }
        }

        public BulkOrderDeedMaterialFilterViewModel _SelectedBulkOrderDeedMaterialFilter = BulkOrderDeedMaterialFilterViewModel.None;
        public BulkOrderDeedMaterialFilterViewModel SelectedBulkOrderDeedMaterialFilter
        {
            get { return _SelectedBulkOrderDeedMaterialFilter; }
            set
            {
                _SelectedBulkOrderDeedMaterialFilter = value;
                _CollectionFilterParameters.BulkOrderDeedMaterial = new BulkOrderDeedMaterialFilter(value?.Value);
                NotifyPropertyChanged(nameof(SelectedBulkOrderDeedMaterialFilter));
            }
        }

        public ObservableCollection<BulkOrderDeedMaterialFilterViewModel> _BulkOrderDeedMaterialFilters;
        public ObservableCollection<BulkOrderDeedMaterialFilterViewModel> BulkOrderDeedMaterialFilters
        {
            get { return _BulkOrderDeedMaterialFilters; }
            set
            {
                _BulkOrderDeedMaterialFilters = value;
                NotifyPropertyChanged(nameof(BulkOrderDeedMaterialFilters));
            }
        }

        public string _BulkOrderDeedNameFilter = String.Empty;
        public string BulkOrderDeedNameFilter
        {
            get { return _BulkOrderDeedNameFilter; }
            set
            {
                _BulkOrderDeedNameFilter = value;
                _CollectionFilterParameters.BulkOrderDeedName = new BulkOrderDeedNameFilter(value);
                NotifyPropertyChanged(nameof(BulkOrderDeedNameFilter));
            }
        }

        public VendorFilterViewModel _SelectedVendorFilter = VendorFilterViewModel.None;
        public VendorFilterViewModel SelectedVendorFilter
        {
            get { return _SelectedVendorFilter; }
            set
            {
                _SelectedVendorFilter = value;
                _CollectionFilterParameters.Vendor = new VendorFilter(value?.Value);
                SetupBulkOrderDeedBookFilters();
                NotifyPropertyChanged(nameof(SelectedVendorFilter));
            }
        }

        public ObservableCollection<VendorFilterViewModel> _VendorFilters;
        public ObservableCollection<VendorFilterViewModel> VendorFilters
        {
            get { return _VendorFilters; }
            set
            {
                _VendorFilters = value;
                NotifyPropertyChanged(nameof(VendorFilters));
            }
        }

        public BulkOrderDeedBookFilterViewModel _SelectedBulkOrderDeedBookFilter = BulkOrderDeedBookFilterViewModel.None;
        public BulkOrderDeedBookFilterViewModel SelectedBulkOrderDeedBookFilter
        {
            get { return _SelectedBulkOrderDeedBookFilter; }
            set
            {
                _SelectedBulkOrderDeedBookFilter = value;
                _CollectionFilterParameters.BulkOrderDeedBook = new BulkOrderDeedBookFilter(value?.Value);
                NotifyPropertyChanged(nameof(SelectedBulkOrderDeedBookFilter));
            }
        }

        public ObservableCollection<BulkOrderDeedBookFilterViewModel> _BulkOrderDeedBookFilters;
        public ObservableCollection<BulkOrderDeedBookFilterViewModel> BulkOrderDeedBookFilters
        {
            get { return _BulkOrderDeedBookFilters; }
            set
            {
                _BulkOrderDeedBookFilters = value;
                NotifyPropertyChanged(nameof(BulkOrderDeedBookFilters));
            }
        }

        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                if (_CurrentPage == value)
                {
                    return;
                }

                _CurrentPage = value;
                NotifyPropertyChanged(nameof(CurrentPage));
                CommandManager.InvalidateRequerySuggested();
                DisplayResults();
            }
        }

        private int _TotalPages;
        public int TotalPages
        {
            get { return _TotalPages; }
            set
            {
                if (_TotalPages == value)
                {
                    return;
                }

                _TotalPages = value;
                NotifyPropertyChanged(nameof(TotalPages));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<ImportPluginViewModel> _ImportPlugins;
        public ObservableCollection<ImportPluginViewModel> ImportPlugins
        {
            get { return _ImportPlugins; }
            set
            {
                _ImportPlugins = value;
                NotifyPropertyChanged(nameof(ImportPlugins));
            }
        }

        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public CollectionViewModel()
        {
            BackCommandVisibility = Visibility.Visible;
            CollectionCommandVisibility = Visibility.Collapsed;
            FirstPageCommand = new RelayCommand(OnFirstPageCommand, FirstPageCommandEnabled);
            PreviousPageCommand = new RelayCommand(OnPreviousPageCommand, PreviousCommandEnabled);
            NextPageCommand = new RelayCommand(OnNextPageCommand, NextCommandEnabled);
            LastPageCommand = new RelayCommand(OnLastPageCommand, LastPageCommandEnabled);
            ApplyFilterCommand = new RelayCommand(OnApplyFilterCommand);
            ClearFilterCommand = new RelayCommand(OnClearFilterCommand);

            _CollectionFilterParameters = new CollectionFilterParameters();

            SetupFilters();

            var bulkOrderDeeds = new List<BulkOrderDeedViewModel>();

            foreach (var bulkOrderDeed in BulkOrderDeedManager.Instance.GetFilteredCollection(_CollectionFilterParameters))
            {
                if (bulkOrderDeed is SmallCollectionBulkOrderDeed smallCollectionBulkOrderDeed)
                {
                    bulkOrderDeeds.Add(new SmallBulkOrderDeedViewModel(smallCollectionBulkOrderDeed));
                }
                else if (bulkOrderDeed is LargeCollectionBulkOrderDeed largeCollectionBulkOrderDeed)
                {
                    bulkOrderDeeds.Add(new LargeBulkOrderDeedViewModel(largeCollectionBulkOrderDeed));
                }
            }

            _AllBulkOrderDeeds = new List<BulkOrderDeedViewModel>();
            _BulkOrderDeeds = new ObservableCollection<BulkOrderDeedViewModel>();
            RefreshIfNecessary();

            BulkOrderDeedManager.Instance.BulkOrderDeedCollectionItemsAdded += BulkOrderDeedCollectionItemsAdded;
            BulkOrderDeedManager.Instance.BulkOrderDeedCollectionItemsRemoved += BulkOrderDeedCollectionItemsRemoved;
            BulkOrderDeedManager.Instance.VendorAdded += VendorAdded;
            BulkOrderDeedManager.Instance.VendorRemoved += VendorRemoved;
            BulkOrderDeedManager.Instance.BulkOrderDeedBookAdded += BulkOrderDeedBookAdded;
            BulkOrderDeedManager.Instance.BulkOrderDeedBookRemoved += BulkOrderDeedBookRemoved;

            var importPlugins = new List<ImportPluginViewModel>();

            foreach (var importPlugin in BulkOrderDeedManager.Instance.ImportPlugins)
            {
                var importPluginViewModel = new ImportPluginViewModel(importPlugin);

                importPluginViewModel.ImportCompleted += ImportCompleted; 
                importPlugins.Add(importPluginViewModel);
            }

            _ImportPlugins = new ObservableCollection<ImportPluginViewModel>(importPlugins);
        }

        private void SetupFilters()
        {
            var professions = new List<ProfessionFilterViewModel>();

            professions.Add(ProfessionFilterViewModel.None);

            foreach (var profession in BulkOrderDeedManager.Instance.Professions)
            {
                professions.Add(new ProfessionFilterViewModel(profession));
            }

            _ProfessionFilters = new ObservableCollection<ProfessionFilterViewModel>(professions);

            _BulkOrderDeedTypeFilters = new ObservableCollection<BulkOrderDeedTypeFilterViewModel>();
            _BulkOrderDeedTypeFilters.Add(BulkOrderDeedTypeFilterViewModel.None);
            _BulkOrderDeedTypeFilters.Add(new BulkOrderDeedTypeFilterViewModel(BulkOrderDeedType.Small));
            _BulkOrderDeedTypeFilters.Add(new BulkOrderDeedTypeFilterViewModel(BulkOrderDeedType.Large));

            _ExceptionalFilters = new ObservableCollection<ExceptionalFilterViewModel>();
            _ExceptionalFilters.Add(ExceptionalFilterViewModel.None);
            _ExceptionalFilters.Add(new ExceptionalFilterViewModel(false));
            _ExceptionalFilters.Add(new ExceptionalFilterViewModel(true));

            _QuantityFilters = new ObservableCollection<QuantityFilterViewModel>();
            _QuantityFilters.Add(QuantityFilterViewModel.None);

            foreach (var quantity in BulkOrderDeedManager.PossibleQuantities)
            {
                _QuantityFilters.Add(new QuantityFilterViewModel(quantity));
            }

            SetupMaterialFilters();
            SetupVendorFilters();
            SetupBulkOrderDeedBookFilters();
        }

        private void SetupMaterialFilters()
        {
            var previousSelectedMaterial = _SelectedBulkOrderDeedMaterialFilter?.Value;

            if (BulkOrderDeedMaterialFilters == null)
            {
                BulkOrderDeedMaterialFilters = new ObservableCollection<BulkOrderDeedMaterialFilterViewModel>();
            }
            else
            {
                BulkOrderDeedMaterialFilters.Clear();
            }

            BulkOrderDeedMaterialFilters.Add(BulkOrderDeedMaterialFilterViewModel.None);

            if (_SelectedProfessionFilter == ProfessionFilterViewModel.None)
            {
                var addedMaterialNames = new List<string>();

                foreach (var profession in BulkOrderDeedManager.Instance.Professions)
                {
                    if (profession.BulkOrderDeedMaterials == null) continue;

                    foreach (var bulkOrderDeedMaterial in profession.BulkOrderDeedMaterials.Materials)
                    {
                        if (addedMaterialNames.Contains(bulkOrderDeedMaterial.Name)) continue;

                        BulkOrderDeedMaterialFilters.Add(new BulkOrderDeedMaterialFilterViewModel(bulkOrderDeedMaterial));
                        addedMaterialNames.Add(bulkOrderDeedMaterial.Name);
                    }
                }
            }
            else
            {
                if (_SelectedProfessionFilter.Value?.BulkOrderDeedMaterials != null)
                {
                    foreach (var bulkOrderDeedMaterial in _SelectedProfessionFilter.Value.BulkOrderDeedMaterials.Materials)
                    {
                        BulkOrderDeedMaterialFilters.Add(new BulkOrderDeedMaterialFilterViewModel(bulkOrderDeedMaterial));
                    }
                }
            }

            var foundPrevious = false;

            if (!String.IsNullOrEmpty(previousSelectedMaterial?.Name))
            {
                foreach (var bulkOrderDeedMaterialFilter in BulkOrderDeedMaterialFilters)
                {
                    if (String.Compare(bulkOrderDeedMaterialFilter.Value?.Name, previousSelectedMaterial.Name, true) == 0)
                    {
                        foundPrevious = true;
                        SelectedBulkOrderDeedMaterialFilter = bulkOrderDeedMaterialFilter;
                        break;
                    }
                }
            }

            if (!foundPrevious)
            {
                SelectedBulkOrderDeedMaterialFilter = BulkOrderDeedMaterialFilterViewModel.None;
            }
        }

        private void SetupVendorFilters()
        {
            var previousSelectedVendor = _SelectedVendorFilter?.Value;

            if (VendorFilters == null)
            {
                VendorFilters = new ObservableCollection<VendorFilterViewModel>();
            }
            else
            {
                VendorFilters.Clear();
            }

            VendorFilters.Add(VendorFilterViewModel.None);
            VendorFilters.Add(VendorFilterViewModel.NoVendor);

            foreach (var vendor in BulkOrderDeedManager.Instance.Vendors)
            {
                VendorFilters.Add(new VendorFilterViewModel(vendor));
            }

            var foundPrevious = false;

            foreach (var vendorFilter in VendorFilters)
            {
                if (vendorFilter.Value == previousSelectedVendor)
                {
                    foundPrevious = true;
                    SelectedVendorFilter = vendorFilter;
                    break;
                }
            }

            if (!foundPrevious)
            {
                SelectedVendorFilter = VendorFilterViewModel.None;
            }
        }

        private void SetupBulkOrderDeedBookFilters()
        {
            var previousSelectedBook = _SelectedBulkOrderDeedBookFilter?.Value;

            if (BulkOrderDeedBookFilters == null)
            {
                BulkOrderDeedBookFilters = new ObservableCollection<BulkOrderDeedBookFilterViewModel>();
            }
            else
            {
                BulkOrderDeedBookFilters.Clear();
            }

            BulkOrderDeedBookFilters.Add(BulkOrderDeedBookFilterViewModel.None);
            BulkOrderDeedBookFilters.Add(BulkOrderDeedBookFilterViewModel.NoBook);

            var bulkOrderDeedBookCollection = ((_SelectedVendorFilter == null) || (_SelectedVendorFilter == VendorFilterViewModel.None) || (_SelectedVendorFilter == VendorFilterViewModel.NoVendor))
                ? BulkOrderDeedManager.Instance.BulkOrderDeedBooks
                : _SelectedVendorFilter.Value.BulkOrderDeedBooks;

            foreach (var bulkOrderDeedBook in bulkOrderDeedBookCollection)
            {
                BulkOrderDeedBookFilters.Add(new BulkOrderDeedBookFilterViewModel(bulkOrderDeedBook));
            }

            var foundPrevious = false;

            foreach (var bulkOrderDeedBookFilter in BulkOrderDeedBookFilters)
            {
                if (bulkOrderDeedBookFilter.Value == previousSelectedBook)
                {
                    foundPrevious = true;
                    SelectedBulkOrderDeedBookFilter = bulkOrderDeedBookFilter;
                    break;
                }
            }

            if (!foundPrevious)
            {
                SelectedBulkOrderDeedBookFilter = BulkOrderDeedBookFilterViewModel.None;
            }
        }

        private bool FirstPageCommandEnabled()
        {
            return CurrentPage != 1;
        }

        private void OnFirstPageCommand(object parameter)
        {
            CurrentPage = 1;
        }

        private bool PreviousCommandEnabled()
        {
            var newPage = _CurrentPage - 1;

            return newPage > 0;
        }

        private void OnPreviousPageCommand(object parameter)
        {
            var newPage = _CurrentPage - 1;

            if (newPage < 1)
            {
                return;
            }

            CurrentPage = newPage;
        }

        private bool NextCommandEnabled()
        {
            var newPage = _CurrentPage + 1;

            return newPage <= TotalPages;
        }

        private void OnNextPageCommand(object parameter)
        {
            var newPage = _CurrentPage + 1;

            if (newPage > TotalPages)
            {
                return;
            }

            CurrentPage = newPage;
        }

        private bool LastPageCommandEnabled()
        {
            return CurrentPage < TotalPages;
        }

        private void OnLastPageCommand(object parameter)
        {
            CurrentPage = TotalPages;
        }

        private void OnApplyFilterCommand(object parameter)
        {
            _NeedsRefresh = true;
            RefreshIfNecessary();
        }

        private void ImportCompleted(object sender, EventArgs e)
        {
            RefreshIfNecessary();
        }

        private void OnClearFilterCommand(object parameter)
        {
            SelectedProfessionFilter = ProfessionFilterViewModel.None;
            SelectedBulkOrderDeedTypeFilter = BulkOrderDeedTypeFilterViewModel.None;
            SelectedExceptionalFilter = ExceptionalFilterViewModel.None;
            SelectedQuantityFilter = QuantityFilterViewModel.None;
            SelectedBulkOrderDeedMaterialFilter = BulkOrderDeedMaterialFilterViewModel.None;
            SelectedVendorFilter = VendorFilterViewModel.None;
            SelectedBulkOrderDeedBookFilter = BulkOrderDeedBookFilterViewModel.None;
            BulkOrderDeedNameFilter =  String.Empty;

            _NeedsRefresh = true;
            RefreshIfNecessary();
        }

        private void BulkOrderDeedCollectionItemsAdded(object sender, BulkOrderDeedEventArgs e)
        {
            _NeedsRefresh = true;
        }

        private void BulkOrderDeedCollectionItemsRemoved(object sender, BulkOrderDeedEventArgs e)
        {
            _NeedsRefresh = true;
        }

        private void VendorAdded(object sender, VendorEventArgs e)
        {
            e.Vendor.BulkOrderDeedBookAdded += BulkOrderDeedBookAdded;
            e.Vendor.BulkOrderDeedBookRemoved += BulkOrderDeedBookRemoved;
            SetupVendorFilters();
            SetupBulkOrderDeedBookFilters();
        }

        private void VendorRemoved(object sender, VendorEventArgs e)
        {
            e.Vendor.BulkOrderDeedBookAdded -= BulkOrderDeedBookAdded;
            e.Vendor.BulkOrderDeedBookRemoved -= BulkOrderDeedBookRemoved;
            SetupVendorFilters();
            SetupBulkOrderDeedBookFilters();
        }

        private void BulkOrderDeedBookAdded(object sender, BulkOrderDeedBookEventArgs e)
        {
            SetupBulkOrderDeedBookFilters();
        }

        private void BulkOrderDeedBookRemoved(object sender, BulkOrderDeedBookEventArgs e)
        {
            SetupBulkOrderDeedBookFilters();
        }

        internal void RefreshIfNecessary()
        {
            if (!_NeedsRefresh) return;

            var bulkOrderDeeds = new List<BulkOrderDeedViewModel>();

            foreach (var bulkOrderDeed in BulkOrderDeedManager.Instance.GetFilteredCollection(_CollectionFilterParameters))
            {
                if (bulkOrderDeed is SmallCollectionBulkOrderDeed smallCollectionBulkOrderDeed)
                {
                    bulkOrderDeeds.Add(new SmallBulkOrderDeedViewModel(smallCollectionBulkOrderDeed));
                }
                else if (bulkOrderDeed is LargeCollectionBulkOrderDeed largeCollectionBulkOrderDeed)
                {
                    bulkOrderDeeds.Add(new LargeBulkOrderDeedViewModel(largeCollectionBulkOrderDeed));
                }
            }

            _AllBulkOrderDeeds.Clear();
            _AllBulkOrderDeeds.AddRange(bulkOrderDeeds);
            TotalPages = (int)Math.Ceiling((double)_AllBulkOrderDeeds.Count / (double)_PageSize);
            
            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
                // No need to call DisplayResults() here since the CurrentPage setter calls it.
            }
            else
            {
                DisplayResults();
            }
            
            _NeedsRefresh = false;
        }

        private void DisplayResults()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                BulkOrderDeeds.Clear();

                var start = (_CurrentPage - 1) * _PageSize;
                var items = _AllBulkOrderDeeds.Skip(start).Take(_PageSize);

                foreach (var bulkOrderDeed in items)
                {
                    BulkOrderDeeds.Add(bulkOrderDeed);
                }
            }));
        }
    }
}
