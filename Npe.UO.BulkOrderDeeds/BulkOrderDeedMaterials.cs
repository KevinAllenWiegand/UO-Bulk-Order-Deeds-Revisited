using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedMaterials
    {
        private const string _MaterialXPath = "Materials/Material";

        private readonly List<BulkOrderDeedMaterial> _Materials;

        public IEnumerable<BulkOrderDeedMaterial> Materials => _Materials.AsReadOnly();

        internal BulkOrderDeedMaterials(string path)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            var materialNodes = xmlDocument.SelectNodes(_MaterialXPath);

            if (materialNodes == null)
            {
                throw new Exception($"Unable to find any defined materials in {path}.");
            }

            _Materials = new List<BulkOrderDeedMaterial>();

            foreach (var materialNode in materialNodes.OfType<XmlNode>())
            {
                _Materials.Add(new BulkOrderDeedMaterial(materialNode));
            }
        }
    }
}
