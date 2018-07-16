using Npe.UO.BulkOrderDeeds.Internal;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds.Filters
{
    public class CollectionFilterParameters : ICloneable<CollectionFilterParameters>
    {
        public ProfessionFilter Profession { get; set; }
        public StringFilter BulkOrderDeedName { get; set; }
        public IntegerFilter Quantity { get; set; }
        public BooleanFilter Exceptional { get; set; }
        public VendorFilter Vendor { get; set; }
        public BulkOrderDeedBookFilter BulkOrderDeedBook { get; set; }
        public BulkOrderDeedTypeFilter BulkOrderDeedType { get; set; }

        public CollectionFilterParameters()
            : this(new ProfessionFilter(), new StringFilter(), new IntegerFilter(), new BooleanFilter(), new VendorFilter(), new BulkOrderDeedBookFilter(), new BulkOrderDeedTypeFilter())
        {
        }

        private CollectionFilterParameters(ProfessionFilter profession, StringFilter bulkOrderDeedName, IntegerFilter quantity, BooleanFilter exceptional, VendorFilter vendor, BulkOrderDeedBookFilter bulkOrderDeedBook, BulkOrderDeedTypeFilter bulkOrderDeedType)
        {
            Profession = profession;
            BulkOrderDeedName = bulkOrderDeedName;
            Quantity = quantity;
            Exceptional = exceptional;
            Vendor = vendor;
            BulkOrderDeedBook = bulkOrderDeedBook;
            BulkOrderDeedType = bulkOrderDeedType;
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

            return retVal;
        }

        public CollectionFilterParameters Clone()
        {
            return new CollectionFilterParameters(Profession, BulkOrderDeedName, Quantity, Exceptional, Vendor, BulkOrderDeedBook, BulkOrderDeedType);
        }
    }
}
