using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class SmallBulkOrderDeedDefinition : BulkOrderDeedDefinition
    {
        private const string _NameAttributeName = "name";
        private const string _CanBeExceptionalAttributeName = "canBeExceptional";
        private const string _CanHaveMaterialAttributeName = "canHaveMaterial";
        private const string _RestrictedToMaterialsXPath = "RestrictedToMaterials/Material";

        private readonly List<string> _RestrictedToMaterials;

        public override string DisplayName => Name;
        public string Name { get; private set; }
        public override bool CanBeExceptional { get; }
        public override bool CanHaveMaterial { get; }
        public IEnumerable<string> RestrictedToMaterials => _RestrictedToMaterials.AsReadOnly();

        internal SmallBulkOrderDeedDefinition(XmlNode xmlNode)
            : base(xmlNode)
        {
            Name = XmlHelper.GetAttributeValue<string>(xmlNode, _NameAttributeName);
            CanBeExceptional = XmlHelper.GetAttributeValue<bool>(xmlNode, _CanBeExceptionalAttributeName);
            CanHaveMaterial = XmlHelper.GetAttributeValue<bool>(xmlNode, _CanHaveMaterialAttributeName);

            _RestrictedToMaterials = new List<string>();

            var materialNodes = xmlNode.SelectNodes(_RestrictedToMaterialsXPath);

            if (materialNodes != null)
            {
                foreach (var materialNode in materialNodes.OfType<XmlNode>())
                {
                    var material = XmlHelper.GetAttributeValue<string>(materialNode, _NameAttributeName);

                    if (!String.IsNullOrEmpty(material) && !_RestrictedToMaterials.Contains(material))
                    {
                        _RestrictedToMaterials.Add(material);
                    }
                }
            }
        }

        public override IEnumerable<BulkOrderDeedMaterial> GetUsableMaterials(IEnumerable<BulkOrderDeedMaterial> bulkOrderDeedMaterials)
        {
            var retVal = new List<BulkOrderDeedMaterial>();

            if (bulkOrderDeedMaterials != null)
            {
                foreach (var material in bulkOrderDeedMaterials)
                {
                    if (IsNotRestrictedFromMaterial(material))
                    {
                        retVal.Add(material);
                    }
                }
            }

            return retVal;
        }

        public override bool IsNotRestrictedFromMaterial(BulkOrderDeedMaterial bulkOrderDeedMaterial)
        {
            var retVal = true;

            if (RestrictedToMaterials.Any())
            {
                retVal = RestrictedToMaterials.Contains(bulkOrderDeedMaterial.Name);
            }

            return retVal;
        }

        public override int CalculatePoints(Profession profession, int quantity, BulkOrderDeedMaterial material, bool isExceptional)
        {
            PointTableEntry entry = null;

            foreach (var pointTableEntry in profession.PointTable.PointTableEntries)
            {
                // Make sure we are looking at the correct type of bulk order.
                if (!(pointTableEntry is SmallBulkOrderPointTableEntry))
                {
                    continue;
                }

                // Check quality, quantity.
                if ((pointTableEntry.Exceptional != isExceptional) || (pointTableEntry.Quantity != quantity))
                {
                    continue;
                }

                // Check that the material matches if it's set.
                if ((material != null) && (String.Compare(material.Name, pointTableEntry.Material, true) != 0))
                {
                    continue;
                }

                if (String.IsNullOrEmpty(pointTableEntry.Overrides))
                {
                    // If we get there, then it was a match; however, we can't just stop here because there could be an override further down the list,
                    // so we have to keep checking, and let the override take over if need be.
                    entry = pointTableEntry;
                }
                else
                {
                    // Override entry check.
                    if ((String.Compare(Name, pointTableEntry.Overrides) == 0))
                    {
                        entry = pointTableEntry;
                        break;
                    }
                }
            }

            return entry != null ? entry.Points : 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
