namespace Npe.UO.BulkOrderDeeds.Filters
{
    public interface IBulkOrderDeedFilter
    {
        bool ApplyFilter(CollectionBulkOrderDeed bulkOrderDeed);
    }
}
