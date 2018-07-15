using System.Collections.Generic;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public abstract class BulkOrderDeedDefinition
    {
        public abstract string DisplayName { get; }
        public abstract bool CanBeExceptional { get; }
        public abstract bool CanHaveMaterial { get; }

        internal BulkOrderDeedDefinition(XmlNode xmlNode)
        {
        }

        public abstract IEnumerable<BulkOrderDeedMaterial> GetUsableMaterials(IEnumerable<BulkOrderDeedMaterial> bulkOrderDeedMaterials);
        public abstract bool IsNotRestrictedFromMaterial(BulkOrderDeedMaterial bulkOrderDeedMaterial);
        public abstract int CalculatePoints(Profession profession, int quantity, BulkOrderDeedMaterial material, bool isExceptional);
    }
}
