namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedPointEntry
    {
        private readonly BulkOrderDeedDefinition _BulkOrderDeedDefinition;

        public string Name => _BulkOrderDeedDefinition.DisplayName;
        public string TypeText => (_BulkOrderDeedDefinition is SmallBulkOrderDeedDefinition) ? "Small" : "Large";
        public int Quantity { get; }
        public bool Quality { get; }
        public string Material { get; }
        public int Points { get; }
        public int PointDifference { get; }

        public BulkOrderDeedPointEntry(BulkOrderDeedDefinition bulkOrderDeedDefinition, int quantity, bool quality, BulkOrderDeedMaterial material, int points, int pointDifference)
        {
            _BulkOrderDeedDefinition = bulkOrderDeedDefinition;
            Quantity = quantity;
            Quality = quality;
            Material = material != null ? material.Name : "None";
            Points = points;
            PointDifference = pointDifference;
        }
    }
}
