using Npe.UO.BulkOrderDeeds;
using Npe.UO.BulkOrderDeeds.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UO_Bulk_Order_Deeds.ViewModels
{
    public class CollectionFilterParametersViewModel : ViewModelBase
    {
        public CollectionFilterParameters CollectionFilterParameters { get; }

        public CollectionFilterParametersViewModel()
        {
            CollectionFilterParameters = new CollectionFilterParameters();
        }
    }
}
