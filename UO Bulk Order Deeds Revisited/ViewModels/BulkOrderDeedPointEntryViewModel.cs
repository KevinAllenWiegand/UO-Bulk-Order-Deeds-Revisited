using Npe.UO.BulkOrderDeeds;
using System.Windows.Media;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedPointEntryViewModel
    {
        private readonly BulkOrderDeedPointEntry _BulkOrderDeedPointEntry;
        private static Brush _DefaultBackgroundColor = new SolidColorBrush(Colors.Black);
        private static Brush _DefaultForegroundColor = new SolidColorBrush(Colors.White);
        private static Brush _HighlightedBackgroundColor = new SolidColorBrush(Colors.Green);
        private static Brush _HighlightedForegroundColor = new SolidColorBrush(Colors.White);

        public string Name => _BulkOrderDeedPointEntry.Name;
        public string TypeText => _BulkOrderDeedPointEntry.TypeText;
        public int Quantity => _BulkOrderDeedPointEntry.Quantity;
        public bool Quality => _BulkOrderDeedPointEntry.Quality;
        public string Material => _BulkOrderDeedPointEntry.Material;
        public int Points => _BulkOrderDeedPointEntry.Points;
        public int PointDifference => _BulkOrderDeedPointEntry.PointDifference;
        public Brush BackgroundColor => PointDifference == 0 ? _HighlightedBackgroundColor : _DefaultBackgroundColor;
        public Brush ForegroundColor => PointDifference == 0 ? _HighlightedForegroundColor : _DefaultForegroundColor;

        public BulkOrderDeedPointEntryViewModel(BulkOrderDeedPointEntry bulkOrderDeedPointEntry)
        {
            _BulkOrderDeedPointEntry = bulkOrderDeedPointEntry;
        }
    }
}
