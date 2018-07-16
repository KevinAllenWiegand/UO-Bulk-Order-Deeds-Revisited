using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedMaterialFilterViewModel : ViewModelBase
    {
        public static readonly BulkOrderDeedMaterialFilterViewModel None = new BulkOrderDeedMaterialFilterViewModel();

        public BulkOrderDeedMaterial Value { get; }

        private BulkOrderDeedMaterialFilterViewModel()
        {
        }

        public BulkOrderDeedMaterialFilterViewModel(BulkOrderDeedMaterial value)
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
