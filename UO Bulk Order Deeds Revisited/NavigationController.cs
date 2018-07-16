using System;
using System.Collections.Generic;
using System.Linq;
using UO_Bulk_Order_Deeds.ViewModels;
using Npe.UO.BulkOrderDeeds;

namespace UO_Bulk_Order_Deeds
{

    public class NavigationController
    {
        #region Singleton

        private static readonly object _SyncRoot = new object();
        private static volatile NavigationController _Instance;

        public static NavigationController Instance
        {
            get
            {
                if (_Instance != null)
                {
                    return _Instance;
                }

                lock (_SyncRoot)
                {
                    if (_Instance == null)
                    {
                        _Instance = new NavigationController();
                    }
                }

                return _Instance;
            }
        }

        private NavigationController()
        {
            _ProfessionViewModels = new Dictionary<string, ProfessionViewModel>();
            _BulkOrderDeedsForRewardViewModels = new Dictionary<ProfessionRewardSearchCriteria, BulkOrderDeedsForRewardViewModel>();
            _History = new Stack<ViewModelBase>();
        }

        #endregion

        private ViewModelBase _CurrentViewModel;
        private ProfessionsViewModel _ProfessionsViewModel;
        private HelpViewModel _HelpViewModel;
        private CollectionViewModel _CollectionViewModel;
        private readonly Dictionary<string, ProfessionViewModel> _ProfessionViewModels;
        private readonly Dictionary<ProfessionRewardSearchCriteria, BulkOrderDeedsForRewardViewModel> _BulkOrderDeedsForRewardViewModels;
        private readonly Stack<ViewModelBase> _History;

        public void Navigate(NavigateTypes navigateTypes, object context = null)
        {
            ViewModelBase viewModel = null;

            switch (navigateTypes)
            {
                case NavigateTypes.Back:
                    if (_History.Count > 0)
                    {
                        viewModel = _History.Pop();
                    }

                    if (viewModel == null)
                    {
                        viewModel = _ProfessionsViewModel;
                    }

                    break;
                case NavigateTypes.Help:
                    if (_HelpViewModel == null)
                    {
                        _HelpViewModel = new HelpViewModel();
                    }

                    viewModel = _HelpViewModel;

                    if (_CurrentViewModel != null)
                    {
                        _History.Push(_CurrentViewModel);
                    }

                    break;
                case NavigateTypes.Error:
                    var exception = (Exception)context;

                    viewModel = new ErrorViewModel(exception);
                    break;
                case NavigateTypes.Profession:
                    var professionName = (string)context;
                    ProfessionViewModel professionViewModel;

                    if (!_ProfessionViewModels.TryGetValue(professionName, out professionViewModel))
                    {
                        var profession = BulkOrderDeedManager.Instance.Professions.FirstOrDefault(p => String.Compare(professionName, p.Name, true) == 0);

                        if (profession != null)
                        {
                            professionViewModel = new ProfessionViewModel(profession);
                            _ProfessionViewModels[professionName] = professionViewModel;
                            viewModel = professionViewModel;
                        }
                    }
                    else
                    {
                        viewModel = professionViewModel;
                    }

                    if (viewModel != null && _CurrentViewModel != null)
                    {
                        _History.Push(_CurrentViewModel);
                    }

                    break;
                case NavigateTypes.BulkOrderDeedsForReward:
                    var professionRewardSearchCriteria = (ProfessionRewardSearchCriteria)context;
                    BulkOrderDeedsForRewardViewModel bulkOrderDeedsForRewardViewModel;

                    if (!_BulkOrderDeedsForRewardViewModels.TryGetValue(professionRewardSearchCriteria, out bulkOrderDeedsForRewardViewModel))
                    {
                        bulkOrderDeedsForRewardViewModel = new BulkOrderDeedsForRewardViewModel(professionRewardSearchCriteria);
                        _BulkOrderDeedsForRewardViewModels[professionRewardSearchCriteria] = bulkOrderDeedsForRewardViewModel;
                    }

                    viewModel = bulkOrderDeedsForRewardViewModel;

                    if (viewModel != null && _CurrentViewModel != null)
                    {
                        _History.Push(_CurrentViewModel);
                    }

                    break;
                case NavigateTypes.BulkOrderDeedCollection:
                    if (_CollectionViewModel == null)
                    {
                        _CollectionViewModel = new CollectionViewModel();
                    }

                    _CollectionViewModel.RefreshIfNecessary();
                    viewModel = _CollectionViewModel;

                    if (_CurrentViewModel != null)
                    {
                        _History.Push(_CurrentViewModel);
                    }

                    break;
                case NavigateTypes.AddBulkOrderDeedToCollection:
                    var collectionBulkOrderDeed = (CollectionBulkOrderDeed)context;

                    viewModel = new AddBulkOrderDeedToCollectionViewModel(collectionBulkOrderDeed);

                    if (_CurrentViewModel != null)
                    {
                        _History.Push(_CurrentViewModel);
                    }

                    break;
                default:
                case NavigateTypes.Professions:
                    try
                    {
                        if (_ProfessionsViewModel == null)
                        {
                            _ProfessionsViewModel = new ProfessionsViewModel();
                        }

                        viewModel = _ProfessionsViewModel;
                    }
                    catch (Exception ex)
                    {
                        Navigate(NavigateTypes.Error, ex);
                    }

                    _History.Clear();

                    break;
            }

            if (viewModel != null)
            {
                _CurrentViewModel = viewModel;
                App.Current.MainWindow.DataContext = _CurrentViewModel;
            }
        }
    }
}
