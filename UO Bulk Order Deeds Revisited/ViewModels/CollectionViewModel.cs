using Npe.UO.BulkOrderDeeds;
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
        private CollectionFilterParametersViewModel _CollectionFilterParametersViewModel;
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

        private int _CurrentPage;
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

        public int TotalPages { get; private set; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public CollectionViewModel()
        {
            BackCommandVisibility = System.Windows.Visibility.Visible;
            CollectionCommandVisibility = System.Windows.Visibility.Collapsed;
            FirstPageCommand = new RelayCommand(OnFirstPageCommand, FirstPageCommandEnabled);
            PreviousPageCommand = new RelayCommand(OnPreviousPageCommand, PreviousCommandEnabled);
            NextPageCommand = new RelayCommand(OnNextPageCommand, NextCommandEnabled);
            LastPageCommand = new RelayCommand(OnLastPageCommand, LastPageCommandEnabled);

            _CollectionFilterParametersViewModel = new CollectionFilterParametersViewModel();

            var bulkOrderDeeds = new List<BulkOrderDeedViewModel>();

            foreach (var bulkOrderDeed in BulkOrderDeedManager.Instance.GetFilteredCollection(_CollectionFilterParametersViewModel.CollectionFilterParameters))
            {
                bulkOrderDeeds.Add(new BulkOrderDeedViewModel(bulkOrderDeed));
            }

            _AllBulkOrderDeeds = new List<BulkOrderDeedViewModel>();
            _BulkOrderDeeds = new ObservableCollection<BulkOrderDeedViewModel>();
            TotalPages = (int)Math.Ceiling((double)_AllBulkOrderDeeds.Count / (double)_PageSize);
            CurrentPage = 1;
            RefreshIfNecessary();

            BulkOrderDeedManager.Instance.BulkOrderDeedCollectionItemAdded += BulkOrderDeedCollectionItemAdded;
            BulkOrderDeedManager.Instance.BulkOrderDeedCollectionItemRemoved += BulkOrderDeedCollectionItemRemoved;
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

        private void BulkOrderDeedCollectionItemAdded(object sender, BulkOrderDeedEventArgs e)
        {
            _NeedsRefresh = true;
        }

        private void BulkOrderDeedCollectionItemRemoved(object sender, BulkOrderDeedEventArgs e)
        {
            _NeedsRefresh = true;
        }

        internal void RefreshIfNecessary()
        {
            if (!_NeedsRefresh) return;

            var bulkOrderDeeds = new List<BulkOrderDeedViewModel>();

            foreach (var bulkOrderDeed in BulkOrderDeedManager.Instance.GetFilteredCollection(_CollectionFilterParametersViewModel.CollectionFilterParameters))
            {
                bulkOrderDeeds.Add(new BulkOrderDeedViewModel(bulkOrderDeed));
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
