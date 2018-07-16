using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class QuantityFilterViewModel : ViewModelBase
    {
        public static readonly QuantityFilterViewModel None = new QuantityFilterViewModel();

        public int? Value { get; }

        private QuantityFilterViewModel()
        {
        }

        public QuantityFilterViewModel(int? value)
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
