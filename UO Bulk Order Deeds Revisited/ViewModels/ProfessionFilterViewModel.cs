using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class ProfessionFilterViewModel : ViewModelBase
    {
        public static readonly ProfessionFilterViewModel None = new ProfessionFilterViewModel();

        public Profession Value { get; }

        private ProfessionFilterViewModel()
        {
        }

        public ProfessionFilterViewModel(Profession value)
        {
            Guard.ArgumentNotNull(nameof(value), value);

            Value = value;
        }

        public override string ToString()
        {
            return Value != null ? Value.Name : CollectionFilterParameters.NoFilter;
        }
    }
}
