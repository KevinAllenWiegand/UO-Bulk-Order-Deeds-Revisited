using Npe.UO.BulkOrderDeeds;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UO_Bulk_Order_Deeds.Commands;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class RewardTableEntryViewModel : ViewModelBase
    {
        private static Brush _DefaultBackgroundColor = new SolidColorBrush(Colors.Black);
        private static Brush _DefaultForegroundColor = new SolidColorBrush(Colors.White);
        private static Brush _HighlightedBackgroundColor = new SolidColorBrush(Colors.Green);
        private static Brush _HighlightedForegroundColor = new SolidColorBrush(Colors.White);

        public PointReward PointReward { get; }
        public BitmapImage Icon { get; }

        public string Name => PointReward.Name;
        public int Points => PointReward.Points;
        public Brush BackgroundColor => _IsHighlighted ? _HighlightedBackgroundColor : _DefaultBackgroundColor;
        public Brush ForegroundColor => _IsHighlighted ? _HighlightedForegroundColor : _DefaultForegroundColor;

        private readonly Profession _Profession;
        private bool _IsHighlighted = false;

        public ICommand FindBulkOrderDeedsForRewardCommand { get; }

        public bool IsHighlighted
        {
            get { return _IsHighlighted; }
            set
            {
                _IsHighlighted = value;
                NotifyPropertyChanged(nameof(IsHighlighted));
                NotifyPropertyChanged(nameof(BackgroundColor));
                NotifyPropertyChanged(nameof(ForegroundColor));
            }
        }

        public RewardTableEntryViewModel(PointReward pointReward, Profession profession)
        {
            FindBulkOrderDeedsForRewardCommand = new RelayCommand(OnFindBulkOrderDeedsForRewardCommand);
            PointReward = pointReward;
            _Profession = profession;
            Icon = new BitmapImage(new Uri($"/Professions/{profession.Name}/Icons/{pointReward.Icon}", UriKind.Relative));
        }

        public void UpdateHighlight(bool isHighlighted)
        {
            IsHighlighted = isHighlighted;
        }

        private void OnFindBulkOrderDeedsForRewardCommand(object parameter)
        {
            NavigationController.Instance.Navigate(NavigateTypes.BulkOrderDeedsForReward, new ProfessionRewardSearchCriteria(_Profession, ((RewardTableEntryViewModel)parameter).PointReward));
        }
    }
}
