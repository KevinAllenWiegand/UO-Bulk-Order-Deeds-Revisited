using Npe.UO.BulkOrderDeeds;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class BulkOrderDeedDefinitionViewModel : ViewModelBase
    {
        public BulkOrderDeedDefinition BulkOrderDeedDefinition { get; private set; }

        public string Name
        {
            get
            {
                var retVal = "";

                if (BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
                {
                    retVal = smallBulkOrderDeedDefinition.Name;
                }

                if (BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
                {
                    retVal = largeBulkOrderDeedDefinition.BulkOrderDeedType;
                }

                return retVal;
            }
        }

        public string DisplayName
        {
            get
            {
                var retVal = "";

                if (BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
                {
                    retVal =smallBulkOrderDeedDefinition.Name;
                }

                if (BulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
                {
                    retVal = $"(Large) {largeBulkOrderDeedDefinition.BulkOrderDeedType}";
                }

                return retVal;
            }
        }

        public BulkOrderDeedDefinitionViewModel(BulkOrderDeedDefinition bulkOrderDeedDefinition)
        {
            BulkOrderDeedDefinition = bulkOrderDeedDefinition;
        }
    }
}
