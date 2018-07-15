using Npe.UO.BulkOrderDeeds;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ProfessionsViewModel : ViewModelBase
    {
        private ObservableCollection<ProfessionButtonViewModel> _Professions;
        public ObservableCollection<ProfessionButtonViewModel> Professions
        {
            get { return _Professions; }
            set
            {
                _Professions = value;
                NotifyPropertyChanged(nameof(Professions));
            }
        }

        public ProfessionsViewModel()
        {
            HomeCommandVisibility = Visibility.Collapsed;
            BulkOrderDeedManager.Instance.LoadProfessions();
            BulkOrderDeedManager.Instance.LoadCollection();

            var professionViewModels = new List<ProfessionButtonViewModel>();

            foreach (var profession in BulkOrderDeedManager.Instance.Professions)
            {
                professionViewModels.Add(new ProfessionButtonViewModel(profession));
            }

            Professions = new ObservableCollection<ProfessionButtonViewModel>(professionViewModels);
        }
    }
}
