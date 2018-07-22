using System;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public abstract class ImportableBulkOrderDeed
    {
        public Guid Id { get; }
        public string Profession { get; }
        public bool Exceptional { get; }
        public string Material { get; }
        public string Name { get; }
        public int Quantity { get; }
        public Guid BulkOrderDeedBook { get; }

        protected ImportableBulkOrderDeed(Guid id, string profession, bool exceptional, string material, string name, int quantity, Guid bulkOrderDeedBook)
        {
            Id = id;
            Profession = profession;
            Exceptional = exceptional;
            Material = material;
            Name = name;
            Quantity = quantity;
            BulkOrderDeedBook = bulkOrderDeedBook;
        }
    }
}
