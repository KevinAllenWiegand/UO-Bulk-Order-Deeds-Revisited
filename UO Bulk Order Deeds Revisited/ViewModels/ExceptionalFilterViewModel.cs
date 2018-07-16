using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ExceptionalFilterViewModel : ViewModelBase
    {
        public static readonly ExceptionalFilterViewModel None = new ExceptionalFilterViewModel();

        public bool? Value { get; }

        private ExceptionalFilterViewModel()
        {
        }

        public ExceptionalFilterViewModel(bool? value)
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
