using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class VendorFilterViewModel : ViewModelBase
    {
        public static readonly VendorFilterViewModel None = new VendorFilterViewModel();
        public static readonly VendorFilterViewModel NoVendor = new VendorFilterViewModel(Vendor.None);

        public Vendor Value { get; }

        private VendorFilterViewModel()
        {
        }

        public VendorFilterViewModel(Vendor value)
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
