using Npe.UO.BulkOrderDeeds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ProfessionViewModel : ViewModelBase
    {
        private readonly Profession _Profession;
        private int _Quantity = 10;

        private BulkOrderDeedDefinitionViewModel _SelectedBulkOrderDeed;
        public BulkOrderDeedDefinitionViewModel SelectedBulkOrderDeed
        {
            get { return _SelectedBulkOrderDeed; }
            set
            {
                if (_SelectedBulkOrderDeed == null && value == null) return;
                if (_SelectedBulkOrderDeed != null && value != null && _SelectedBulkOrderDeed.Equals(value)) return;

                _SelectedBulkOrderDeed = value;

                var bulkOrderDeedMaterials = new List<BulkOrderDeedMaterialViewModel>();

                if (_SelectedBulkOrderDeed != null && _Profession.BulkOrderDeedMaterials != null)
                {
                    foreach (var material in _SelectedBulkOrderDeed.BulkOrderDeedDefinition.GetUsableMaterials(_Profession.BulkOrderDeedMaterials.Materials))
                    {
                        bulkOrderDeedMaterials.Add(new BulkOrderDeedMaterialViewModel(material));
                    }
                }

                if (MaterialsChanged(bulkOrderDeedMaterials))
                {
                    Materials = new ObservableCollection<BulkOrderDeedMaterialViewModel>(bulkOrderDeedMaterials);
                    NotifyPropertyChanged(nameof(CanSelectMaterial));
                }

                if (SelectedMaterial == null && bulkOrderDeedMaterials.Count() == 1)
                {
                    SelectedMaterial = bulkOrderDeedMaterials.First();
                }

                if (!QualityIsEnabled())
                {
                    IsExceptional = false;
                }

                NotifyPropertyChanged(nameof(CanSelectQuality));
                NotifyPropertyChanged(nameof(BulkOrderDeedSummary));
                NotifyPropertyChanged(nameof(IsLargeBulkOrderDeed));
                NotifyPropertyChanged(nameof(SelectedBulkOrderDeed));
                CalculateRewards();
            }
        }

        private BulkOrderDeedMaterialViewModel _SelectedMaterial;
        public BulkOrderDeedMaterialViewModel SelectedMaterial
        {
            get { return _SelectedMaterial; }
            set
            {
                if (_SelectedMaterial == null && value == null) return;
                if (_SelectedMaterial != null && value != null && String.Compare(_SelectedMaterial.Name, value.Name) == 0) return;

                _SelectedMaterial = value;
                NotifyPropertyChanged(nameof(SelectedMaterial));
                CalculateRewards();
            }
        }

        public string BulkOrderDeedSummary
        {
            get
            {
                string retVal = null;

                if (_SelectedBulkOrderDeed?.BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
                {
                    retVal = GetLargeBulkOrderDeedItemSummary(largeBulkOrderDeedDefinition);
                }

                if (_SelectedBulkOrderDeed?.BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
                {
                    var largeBulkOrderDeeds = _Profession.BulkOrderDeedDefinitions.Definitions.OfType<LargeBulkOrderDeedDefinition>()
                        .Where(l => l.SmallBulkOrderDeedDefinitions.Where(s => String.Compare(smallBulkOrderDeedDefinition.Name, s.Name, true) == 0).Any());
                    var stringBuilder = new StringBuilder();

                    foreach (var largeBulkOrderDeed in largeBulkOrderDeeds)
                    {
                        stringBuilder.AppendLine(largeBulkOrderDeed.BulkOrderDeedType);
                    }

                    if (stringBuilder.Length > 0)
                    {
                        retVal = stringBuilder.ToString().Trim();
                    }
                    else
                    {
                        retVal = "None";
                    }
                }

                return retVal;
            }
        }

        private string GetLargeBulkOrderDeedItemSummary(LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
        {
            var stringBuilder = new StringBuilder();

            foreach (var smallBulkOrderDeedDefinition in largeBulkOrderDeedDefinition.SmallBulkOrderDeedDefinitions)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder.Append(smallBulkOrderDeedDefinition.Name);
            }

            return stringBuilder.ToString();
        }

        private bool _IsQuantity10 = true;
        public bool IsQuantity10
        {
            get { return _IsQuantity10; }
            set
            {
                if (_IsQuantity10 == value)
                {
                    return;
                }

                _IsQuantity10 = value;
                NotifyPropertyChanged(nameof(IsQuantity10));
            }
        }

        private bool _IsQuantity15;
        public bool IsQuantity15
        {
            get { return _IsQuantity15; }
            set
            {
                if (_IsQuantity15 == value)
                {
                    return;
                }

                _IsQuantity15 = value;
                NotifyPropertyChanged(nameof(IsQuantity15));
            }
        }

        private bool _IsQuantity20;
        public bool IsQuantity20
        {
            get { return _IsQuantity20; }
            set
            {
                if (_IsQuantity20 == value)
                {
                    return;
                }

                _IsQuantity20 = value;
                NotifyPropertyChanged(nameof(IsQuantity20));
            }
        }

        private bool _IsExceptional;
        public bool IsExceptional
        {
            get { return _IsExceptional; }
            set
            {
                if (_IsExceptional == value)
                {
                    return;
                }

                _IsExceptional = value;
                NotifyPropertyChanged(nameof(IsExceptional));
                NotifyPropertyChanged(nameof(IsNotExceptional));
                CalculateRewards();
            }
        }

        private int _Points;
        public int Points
        {
            get { return _Points; }
            set
            {
                if (_Points == value)
                {
                    return;
                }

                _Points = value;
                NotifyPropertyChanged(nameof(Points));
                NotifyPropertyChanged(nameof(BankedPoints));
                NotifyPropertyChanged(nameof(HasPointValue));
            }
        }

        public bool IsNotExceptional => !_IsExceptional;
        public string Name => _Profession.Name;
        public int BulkOrderDeedContainerWidth => (int)Math.Floor((App.Current.MainWindow.ActualWidth - 30) / 2);
        public int RewardTableContainerWidth => (int)Math.Floor((App.Current.MainWindow.ActualWidth - 30) / 2);
        public bool CanSelectMaterial => MaterialIsEnabled();
        public bool CanSelectQuality => QualityIsEnabled();
        public bool IsLargeBulkOrderDeed => _SelectedBulkOrderDeed?.BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition;
        public double BankedPoints => _Points * (IsLargeBulkOrderDeed ? _Profession.LargeBankedPointsFactor : _Profession.SmallBankedPointsFactor);
        public bool HasPointValue => _Points > 0;

        private ObservableCollection<RewardTableEntryViewModel> _RewardTableEntries;
        public ObservableCollection<RewardTableEntryViewModel> RewardTableEntries
        {
            get { return _RewardTableEntries; }
            set
            {
                _RewardTableEntries = value;
                NotifyPropertyChanged(nameof(RewardTableEntries));
            }
        }

        private ObservableCollection<BulkOrderDeedMaterialViewModel> _Materials;
        public ObservableCollection<BulkOrderDeedMaterialViewModel> Materials
        {
            get { return _Materials; }
            set
            {
                _Materials = value;
                NotifyPropertyChanged(nameof(Materials));
            }
        }

        private ObservableCollection<BulkOrderDeedDefinitionViewModel> _BulkOrderDeeds;
        public ObservableCollection<BulkOrderDeedDefinitionViewModel> BulkOrderDeeds
        {
            get { return _BulkOrderDeeds; }
            set
            {
                _BulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(BulkOrderDeeds));
            }
        }

        public ICommand QuantityChangedCommand { get; }
        public ICommand AddToCollectionCommand { get; }

        public ProfessionViewModel(Profession profession)
        {
            App.Current.MainWindow.SizeChanged += MainWindowSizeChanged;

            BackCommandVisibility = Visibility.Visible;
            QuantityChangedCommand = new RelayCommand(OnQuantityChangedCommand);
            AddToCollectionCommand = new RelayCommand(OnAddToCollectionCommand, AddToCollectionCommandEnabled);
            _Profession = profession;

            var rewardTableEntryViewModels = new List<RewardTableEntryViewModel>();

            foreach (var rewardTableEntry in profession.PointRewards.Rewards)
            {
                rewardTableEntryViewModels.Add(new RewardTableEntryViewModel(rewardTableEntry, profession));
            }

            RewardTableEntries = new ObservableCollection<RewardTableEntryViewModel>(rewardTableEntryViewModels);

            var bulkOrderDeedDefinitionViewModels = new List<BulkOrderDeedDefinitionViewModel>();

            foreach (var bulkOrderDeedDefinition in profession.BulkOrderDeedDefinitions.Definitions)
            {
                bulkOrderDeedDefinitionViewModels.Add(new BulkOrderDeedDefinitionViewModel(bulkOrderDeedDefinition));
            }

            BulkOrderDeeds = new ObservableCollection<BulkOrderDeedDefinitionViewModel>(bulkOrderDeedDefinitionViewModels);
        }

        private bool MaterialIsEnabled()
        {
            var retVal = false;

            if (_SelectedBulkOrderDeed != null)
            {
                retVal = _SelectedBulkOrderDeed.BulkOrderDeedDefinition.CanHaveMaterial;
            }

            return retVal;
        }

        private bool QualityIsEnabled()
        {
            var retVal = false;

            if (_SelectedBulkOrderDeed != null)
            {
                retVal = _SelectedBulkOrderDeed.BulkOrderDeedDefinition.CanBeExceptional;
            }

            return retVal;
        }

        protected override void OnBackCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.Professions);
        }

        private void OnQuantityChangedCommand(object parameter)
        {
            _Quantity = (int)Convert.ChangeType(parameter, typeof(int));
            NotifyPropertyChanged(nameof(IsQuantity10));
            NotifyPropertyChanged(nameof(IsQuantity15));
            NotifyPropertyChanged(nameof(IsQuantity20));
            CalculateRewards();
        }

        private void OnAddToCollectionCommand(object parameter)
        {
            if (_SelectedBulkOrderDeed.BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
            {
                NavigationController.Instance.Navigate(NavigateTypes.AddBulkOrderDeedToCollection, new LargeCollectionBulkOrderDeed(_Profession, largeBulkOrderDeedDefinition, _Quantity, _IsExceptional, _SelectedMaterial?.BulkOrderDeedMaterial));
            }
            else if (_SelectedBulkOrderDeed.BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
            {
                NavigationController.Instance.Navigate(NavigateTypes.AddBulkOrderDeedToCollection, new SmallCollectionBulkOrderDeed(_Profession, smallBulkOrderDeedDefinition, _Quantity, _IsExceptional, _SelectedMaterial?.BulkOrderDeedMaterial));
            }
        }

        private bool AddToCollectionCommandEnabled()
        {
            var materialIsEnabled = MaterialIsEnabled();

            return (_SelectedBulkOrderDeed != null && (!materialIsEnabled || (materialIsEnabled && _SelectedMaterial != null)));
        }

        private bool MaterialsChanged(IList<BulkOrderDeedMaterialViewModel> newBulkOrderDeedMaterials)
        {
            var retVal = newBulkOrderDeedMaterials.Count != Materials?.Count;

            if (!retVal)
            {
                foreach (var material in newBulkOrderDeedMaterials)
                {
                    var existing = Materials.FirstOrDefault(m => m.BulkOrderDeedMaterial == material.BulkOrderDeedMaterial);

                    if (existing == null)
                    {
                        retVal = true;
                        break;
                    }
                }
            }

            return retVal;
        }

        private void CalculateRewards()
        {
            Points = 0;

            foreach(var rewardTableEntry in _RewardTableEntries)
            {
                rewardTableEntry.UpdateHighlight(false);
            }

            if (_SelectedBulkOrderDeed == null || (MaterialIsEnabled() && _SelectedMaterial == null))
            {
                return;
            }

            Points = _SelectedBulkOrderDeed.BulkOrderDeedDefinition.CalculatePoints(_Profession, _Quantity, _SelectedMaterial?.BulkOrderDeedMaterial, _IsExceptional);

            foreach (var rewardTableEntry in _RewardTableEntries)
            {
                rewardTableEntry.UpdateHighlight(rewardTableEntry.Points <= Points);
            }
        }

        private void MainWindowSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(BulkOrderDeedContainerWidth));
            NotifyPropertyChanged(nameof(RewardTableContainerWidth));
        }
    }
}
