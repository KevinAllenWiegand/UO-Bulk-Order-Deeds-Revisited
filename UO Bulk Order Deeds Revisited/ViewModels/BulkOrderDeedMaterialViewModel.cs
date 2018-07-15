using Npe.UO.BulkOrderDeeds;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedMaterialViewModel : ViewModelBase
    {
        public BulkOrderDeedMaterial BulkOrderDeedMaterial { get; private set; }

        public string Name => BulkOrderDeedMaterial.Name;

        public BulkOrderDeedMaterialViewModel(BulkOrderDeedMaterial bulkOrderDeedMaterial)
        {
            BulkOrderDeedMaterial = bulkOrderDeedMaterial;
        }
    }
}
