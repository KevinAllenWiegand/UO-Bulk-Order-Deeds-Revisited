using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedBookFilterViewModel : ViewModelBase
    {
        public static readonly BulkOrderDeedBookFilterViewModel None = new BulkOrderDeedBookFilterViewModel();
        public static readonly BulkOrderDeedBookFilterViewModel NoBook = new BulkOrderDeedBookFilterViewModel(BulkOrderDeedBook.None);

        public BulkOrderDeedBook Value { get; }

        private BulkOrderDeedBookFilterViewModel()
        {
        }

        public BulkOrderDeedBookFilterViewModel(BulkOrderDeedBook value)
        {
            Guard.ArgumentNotNull(nameof(value), value);

            Value = value;
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : CollectionFilterParameters.NoFilter;
        }
    }
}
