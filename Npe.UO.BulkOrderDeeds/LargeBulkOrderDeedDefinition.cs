using Npe.UO.BulkOrderDeeds.Internal;
using System.Collections.Generic;
using System.Linq;

using System.Xml;
using System;

namespace Npe.UO.BulkOrderDeeds
{
    public class LargeBulkOrderDeedDefinition : BulkOrderDeedDefinition
    {
        private const string _ItemEntryXPath = "Items/Item";
        private const string _TypeAttributeName = "type";
        private const string _NameAttributeName = "name";

        private bool _CanBeExceptional;
        private bool _CanHaveMaterial;
        private readonly List<string> _SmallBulkOrderDeedNames;
        private List<SmallBulkOrderDeedDefinition> _SmallBulkOrderDeedDefinitions;

        public override string DisplayName => BulkOrderDeedType;
        public override bool CanBeExceptional => _CanBeExceptional;
        public override bool CanHaveMaterial => _CanHaveMaterial;

        public string BulkOrderDeedType { get; private set; }

        public IEnumerable<SmallBulkOrderDeedDefinition> SmallBulkOrderDeedDefinitions => _SmallBulkOrderDeedDefinitions.AsReadOnly();

        internal LargeBulkOrderDeedDefinition(XmlNode xmlNode)
            : base(xmlNode)
        {
            BulkOrderDeedType = XmlHelper.GetAttributeValue<string>(xmlNode, _TypeAttributeName);

            var itemEntryNodes = xmlNode.SelectNodes(_ItemEntryXPath);

            _SmallBulkOrderDeedNames = new List<string>();

            if (itemEntryNodes != null)
            {
                foreach (var itemEntryNode in itemEntryNodes.OfType<XmlNode>())
                {
                    _SmallBulkOrderDeedNames.Add(XmlHelper.GetAttributeValue<string>(itemEntryNode, _NameAttributeName));
                }
            }
        }

        internal void ResolveSmallBulkOrderDeeds(IEnumerable<SmallBulkOrderDeedDefinition> smallBulkOrderDeedDefinitions, Profession profession)
        {
            if (_SmallBulkOrderDeedDefinitions == null)
            {
                _SmallBulkOrderDeedDefinitions = new List<SmallBulkOrderDeedDefinition>();
            }

            var setProperties = false;

            foreach (var name in _SmallBulkOrderDeedNames)
            {
                var smallBulkOrderDeedDefinition = smallBulkOrderDeedDefinitions.FirstOrDefault(bod => String.Compare(name, bod.Name, StringComparison.InvariantCultureIgnoreCase) == 0);

                if (smallBulkOrderDeedDefinition != null)
                {
                    if (!setProperties)
                    {
                        _CanBeExceptional = smallBulkOrderDeedDefinition.CanBeExceptional;
                        _CanHaveMaterial = smallBulkOrderDeedDefinition.CanHaveMaterial;
                        setProperties = true;
                    }

                    _SmallBulkOrderDeedDefinitions.Add(smallBulkOrderDeedDefinition);
                }
                else
                {
                    throw new Exception($"Cannot find a {profession.Name} small bulk order for \"{name}\".");
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

            var firstSmallBulkOrderDeedDefinition = SmallBulkOrderDeedDefinitions.First();

            if (firstSmallBulkOrderDeedDefinition.RestrictedToMaterials.Any())
            {
                retVal = firstSmallBulkOrderDeedDefinition.RestrictedToMaterials.Contains(bulkOrderDeedMaterial.Name);
            }

            return retVal;
        }

        public override int CalculatePoints(Profession profession, int quantity, BulkOrderDeedMaterial material, bool isExceptional)
        {
            PointTableEntry entry = null;

            foreach (var pointTableEntry in profession.PointTable.PointTableEntries)
            {
                // Make sure we are looking at the correct type of bulk order.
                if (!(pointTableEntry is LargeBulkOrderPointTableEntry))
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

                // Check the large bulk order deed name.
                if ((String.Compare(BulkOrderDeedType, ((LargeBulkOrderPointTableEntry)pointTableEntry).BulkOrderDeedType, true) != 0))
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
                    if ((String.Compare(BulkOrderDeedType, pointTableEntry.Overrides) == 0))
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
            return BulkOrderDeedType;
        }
    }
}
