using System;
using System.Collections.Generic;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public class ImportableLargeBulkOrderDeed : ImportableBulkOrderDeed
    {
        private List<string> _Combined;
        public IEnumerable<string> Combined => _Combined.AsReadOnly();
        
        public ImportableLargeBulkOrderDeed(Guid id, string profession, bool exceptional, string material, string name, int quantity, Guid bodBook, IEnumerable<string> combined)
            : base(id, profession, exceptional, material, name, quantity, bodBook)
        {
            _Combined = new List<string>(combined);
        }
    }
}
