using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedTypeFilterViewModel : ViewModelBase
    {
        public static readonly BulkOrderDeedTypeFilterViewModel None = new BulkOrderDeedTypeFilterViewModel();

        public BulkOrderDeedType? Value { get; }

        private BulkOrderDeedTypeFilterViewModel()
        {
        }

        public BulkOrderDeedTypeFilterViewModel(BulkOrderDeedType? value)
        {
            Guard.ArgumentNotNull(nameof(value), value);

            Value = value;
        }

        public override string ToString()
        {
            return Value != null && Value.HasValue ? Value.Value.ToString() : CollectionFilterParameters.NoFilter;
        }
    }
}
