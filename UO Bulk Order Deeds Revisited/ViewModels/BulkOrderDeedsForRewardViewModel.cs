using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedsForRewardViewModel : ViewModelBase
    {
        private readonly ProfessionRewardSearchCriteria _ProfessionRewardSearchCriteria;
        private readonly int _TargetPoints;
        private const int _PageSize = 25;
        private List<BulkOrderDeedPointEntry> _AllBulkOrderDeedPointEntries;

        public string ProfessionName => _ProfessionRewardSearchCriteria.Profession.Name;
        public string Name => _ProfessionRewardSearchCriteria.PointReward.Name;
        public int Points => _ProfessionRewardSearchCriteria.PointReward.Points;
        public BitmapImage Icon { get; }

        private ObservableCollection<BulkOrderDeedPointEntryViewModel> _BulkOrderDeedPointEntries;
        public ObservableCollection<BulkOrderDeedPointEntryViewModel> BulkOrderDeedPointEntries
        {
            get { return _BulkOrderDeedPointEntries; }
            set
            {
                _BulkOrderDeedPointEntries = value;
                NotifyPropertyChanged(nameof(BulkOrderDeedPointEntries));
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

        public BulkOrderDeedsForRewardViewModel(ProfessionRewardSearchCriteria professionRewardSearchCriteria)
        {
            BackCommandVisibility = Visibility.Visible;
            FirstPageCommand = new RelayCommand(OnFirstPageCommand, FirstPageCommandEnabled);
            PreviousPageCommand = new RelayCommand(OnPreviousPageCommand, PreviousCommandEnabled);
            NextPageCommand = new RelayCommand(OnNextPageCommand, NextCommandEnabled);
            LastPageCommand = new RelayCommand(OnLastPageCommand, LastPageCommandEnabled);

            _AllBulkOrderDeedPointEntries = new List<BulkOrderDeedPointEntry>();
            _BulkOrderDeedPointEntries = new ObservableCollection<BulkOrderDeedPointEntryViewModel>();
            _TargetPoints = professionRewardSearchCriteria.PointReward.Points;
            _ProfessionRewardSearchCriteria = professionRewardSearchCriteria;
            Icon = new BitmapImage(new Uri($"/Professions/{_ProfessionRewardSearchCriteria.Profession.Name}/Icons/{professionRewardSearchCriteria.PointReward.Icon}", UriKind.Relative));

            GetResults();
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

        private void GetResults()
        {
            var bulkOrderDeedPointEntries = new List<BulkOrderDeedPointEntry>();
            var profession = _ProfessionRewardSearchCriteria.Profession;

            foreach (var bulkOrderDeedDefinition in profession.BulkOrderDeedDefinitions.Definitions)
            {
                var quantities = new List<int> { 10, 15, 20 };
                var qualities = new List<bool> { false };
                var materials = new List<BulkOrderDeedMaterial>();

                if (bulkOrderDeedDefinition.CanBeExceptional)
                {
                    qualities.Add(true);
                }

                var availableMaterials = bulkOrderDeedDefinition.GetUsableMaterials(profession.BulkOrderDeedMaterials?.Materials);

                if (availableMaterials.Any())
                {
                    materials.AddRange(availableMaterials);
                }

                foreach (var quantity in quantities)
                {
                    foreach (var quality in qualities)
                    {
                        if (materials.Any())
                        {
                            foreach (var material in materials)
                            {
                                var points = bulkOrderDeedDefinition.CalculatePoints(profession, quantity, material, quality);

                                if (points >= _TargetPoints)
                                {
                                    var difference = Math.Abs(_TargetPoints - points);

                                    bulkOrderDeedPointEntries.Add(new BulkOrderDeedPointEntry(bulkOrderDeedDefinition, quantity, quality, material, points, difference));
                                }
                            }
                        }
                        else
                        {
                            var points = bulkOrderDeedDefinition.CalculatePoints(profession, quantity, null, quality);

                            if (points >= _TargetPoints)
                            {
                                var difference = Math.Abs(_TargetPoints - points);

                                bulkOrderDeedPointEntries.Add(new BulkOrderDeedPointEntry(bulkOrderDeedDefinition, quantity, quality, null, points, difference));
                            }
                        }
                    }
                }
            }

            bulkOrderDeedPointEntries.Sort(new BulkOrderDeedPointEntryComparer());

            _AllBulkOrderDeedPointEntries.Clear();
            _AllBulkOrderDeedPointEntries.AddRange(bulkOrderDeedPointEntries);

            TotalPages = (int)Math.Ceiling((double)_AllBulkOrderDeedPointEntries.Count / (double)_PageSize);
            CurrentPage = 1;
        }

        private void DisplayResults()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                BulkOrderDeedPointEntries.Clear();

                var start = (_CurrentPage - 1) * _PageSize;
                var items = _AllBulkOrderDeedPointEntries.Skip(start).Take(_PageSize);

                foreach (var bulkOrderDeedEntryPoint in items)
                {
                    BulkOrderDeedPointEntries.Add(new BulkOrderDeedPointEntryViewModel(bulkOrderDeedEntryPoint));
                }
            }));
        }
    }
}
