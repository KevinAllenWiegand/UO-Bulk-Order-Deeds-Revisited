using Npe.UO.BulkOrderDeeds.Internal;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class CollectionFilterParameters : ICloneable<CollectionFilterParameters>
    {
        public const string NoFilter = "[No Filter]";

        public ProfessionFilter Profession { get; set; }
        public BulkOrderDeedNameFilter BulkOrderDeedName { get; set; }
        public QuantityFilter Quantity { get; set; }
        public ExceptionalFilter Exceptional { get; set; }
        public VendorFilter Vendor { get; set; }
        public BulkOrderDeedBookFilter BulkOrderDeedBook { get; set; }
        public BulkOrderDeedTypeFilter BulkOrderDeedType { get; set; }
        public BulkOrderDeedMaterialFilter BulkOrderDeedMaterial { get; set; }

        public CollectionFilterParameters()
        {
        }

        private CollectionFilterParameters(ProfessionFilter profession, BulkOrderDeedNameFilter bulkOrderDeedName, QuantityFilter quantity, ExceptionalFilter exceptional, VendorFilter vendor, BulkOrderDeedBookFilter bulkOrderDeedBook, BulkOrderDeedTypeFilter bulkOrderDeedType, BulkOrderDeedMaterialFilter bulkOrderDeedMaterial)
        {
            Profession = profession;
            BulkOrderDeedName = bulkOrderDeedName;
            Quantity = quantity;
            Exceptional = exceptional;
            Vendor = vendor;
            BulkOrderDeedBook = bulkOrderDeedBook;
            BulkOrderDeedType = bulkOrderDeedType;
            BulkOrderDeedMaterial = bulkOrderDeedMaterial;
        }

        public IEnumerable<IBulkOrderDeedFilter> GetAppliedFilters()
        {
            var retVal = new List<IBulkOrderDeedFilter>();

            if (Profession != null)
            {
                retVal.Add(Profession);
            }

            if (BulkOrderDeedName != null)
            {
                retVal.Add(BulkOrderDeedName);
            }

            if (Quantity != null)
            {
                retVal.Add(Quantity);
            }

            if (Exceptional != null)
            {
                retVal.Add(Exceptional);
            }

            if (Vendor != null)
            {
                retVal.Add(Vendor);
            }

            if (BulkOrderDeedBook != null)
            {
                retVal.Add(BulkOrderDeedBook);
            }

            if (BulkOrderDeedType != null)
            {
                retVal.Add(BulkOrderDeedType);
            }

            if (BulkOrderDeedMaterial != null)
            {
                retVal.Add(BulkOrderDeedMaterial);
            }

            return retVal;
        }

        public CollectionFilterParameters Clone()
        {
            return new CollectionFilterParameters(Profession, BulkOrderDeedName, Quantity, Exceptional, Vendor, BulkOrderDeedBook, BulkOrderDeedType, BulkOrderDeedMaterial);
        }
    }
}
