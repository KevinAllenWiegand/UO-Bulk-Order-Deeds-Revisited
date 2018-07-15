using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConvertOldBulkOrderDeedDataFiles
{
    class Program
    {
        private const string _OldSmallBulkOrderDeedFile = "OldDataFiles\\SmallBODs.xml";
        private const string _OldLargeBulkOrderDeedFile = "OldDataFiles\\LargeBODs.xml";
        private const string _NewDataFileFolder = "NewDataFiles";
        private static readonly string _NewSmithBulkOrderDeedFile = $"{_NewDataFileFolder}\\Smith_BulkOrderDeeds.xml";
        private static readonly  string _NewTailorBulkOrderDeedFile = $"{_NewDataFileFolder}\\Tailor_BulkOrderDeeds.xml";
        private const string _XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        private const string _NewDataFileRootNodeName = "BulkOrderDeeds";
        private const string _NewDataFileSmallNodeName = "Small";
        private const string _NewDataFileLargeNodeName = "Large";
        private const string _TrueString = "true";
        private const string _FalseString = "false";
        private const string _BulkOrderDeedNodeName = "BulkOrderDeed";
        private static readonly string _SmallBulkOrderDeedSingleLineFormat = $"        <{_BulkOrderDeedNodeName} name=\"{{0}}\" canBeExceptional=\"{{1}}\" canHaveMaterial=\"{{2}}\" />";
        private static readonly string _SmallBulkOrderDeedMultiLineFormat = $"        <{_BulkOrderDeedNodeName} name=\"{{0}}\" canBeExceptional=\"{{1}}\" canHaveMaterial=\"{{2}}\">";
        private const string _RestrictedToMaterialsNodeName = "RestrictedToMaterials";
        private const string _IronMaterialNode = "<Material name=\"Iron\" />";
        private const string _ClothMaterialNode = "<Material name=\"Cloth\" />";
        private const string _MaterialNodeFormat = "                <Material name=\"{0}\" />";
        private static readonly string _LargeBulkOrderDeedFirstLineFormat = $"        <{_BulkOrderDeedNodeName} type=\"{{0}}\">";
        private static readonly string _LargeBulkOrderDeedLastLine = $"        </{_BulkOrderDeedNodeName}>";
        private const string _ItemsNodeName = "Items";
        private static readonly string _ItemsFirstLine = $"            <{_ItemsNodeName}>";
        private static readonly string _ItemsLastLine = $"            </{_ItemsNodeName}>";
        private const string _ItemNodeFormat = "                <Item name=\"{0}\" />";

        private const string _OldSmallSmithBulkOrderDeedXPath = "BODS/Blacksmith/BOD";
        private const string _OldSmallTailorBulkOrderDeedXPath = "BODS/Tailor/BOD";
        private const string _OldLargeSmithBulkOrderDeedMapItemXPath = "Larges/Map/Smith/MapItem";
        private const string _OldLargeSmithBulkOrderDeedItemXPathFormat = "Larges/Smith/{0}/Item";
        private const string _OldLargeTailorBulkOrderDeedMapItemXPath = "Larges/Map/Tailor/MapItem";
        private const string _OldLargeTailorBulkOrderDeedItemXPathFormat = "Larges/Tailor/{0}/Item";

        private static readonly string[] _SmithMaterials = new[]{ "Iron", "Dull Copper", "Shadow Iron", "Copper", "Bronze", "Gold", "Agapite", "Verite", "Valorite" };
        private const string _SmithIronOnlyCategory = "Weapon" ;
        private static readonly string[] _TailorMaterials = new[] { "Cloth", "Normal Leather", "Spined Leather", "Horned Leather", "Barbed Leather" };
        private const string _TailorClothOnlyCategory = "Cloth";

        private static List<OldSmallBulkOrderDeed> _OldSmithSmallBulkOrderDeeds;
        private static List<OldSmallBulkOrderDeed> _OldTailorSmallBulkOrderDeeds;
        private static List<OldLargeBulkOrderDeed> _OldSmithLargeBulkOrderDeeds;
        private static List<OldLargeBulkOrderDeed> _OldTailorLargeBulkOrderDeeds;

        static void Main(string[] args)
        {
            _OldSmithSmallBulkOrderDeeds = new List<OldSmallBulkOrderDeed>();
            _OldTailorSmallBulkOrderDeeds = new List<OldSmallBulkOrderDeed>();
            _OldSmithLargeBulkOrderDeeds = new List<OldLargeBulkOrderDeed>();
            _OldTailorLargeBulkOrderDeeds = new List<OldLargeBulkOrderDeed>();

            try
            {
                // Read in the old data.
                var xmlSmallDocument = new XmlDocument();

                xmlSmallDocument.Load(_OldSmallBulkOrderDeedFile);

                LoadOldSmithSmallBulkOrderDeeds(xmlSmallDocument);
                LoadOldTailorSmallBulkOrderDeeds(xmlSmallDocument);

                var xmlLargeDocument = new XmlDocument();

                xmlLargeDocument.Load(_OldLargeBulkOrderDeedFile);

                LoadOldSmithLargeBulkOrderDeeds(xmlLargeDocument);
                LoadOldTailorLargeBulkOrderDeeds(xmlLargeDocument);

                // Ensure the output folder exists.
                if (!Directory.Exists(_NewDataFileFolder))
                {
                    Directory.CreateDirectory(_NewDataFileFolder);
                }

                // Write the new data.
                WriteNewSmithBulkOrderDeedFile();
                WriteNewTailorBulkOrderDeedFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static void LoadOldSmithSmallBulkOrderDeeds(XmlDocument xmlDocument)
        {
            var xmlNodeList = xmlDocument.SelectNodes(_OldSmallSmithBulkOrderDeedXPath);

            if (xmlNodeList != null)
            {
                foreach (var xmlNode in xmlNodeList.OfType<XmlNode>())
                {
                    _OldSmithSmallBulkOrderDeeds.Add(new OldSmallBulkOrderDeed(xmlNode));
                }
            }
        }

        private static void LoadOldTailorSmallBulkOrderDeeds(XmlDocument xmlDocument)
        {
            var xmlNodeList = xmlDocument.SelectNodes(_OldSmallTailorBulkOrderDeedXPath);

            if (xmlNodeList != null)
            {
                foreach (var xmlNode in xmlNodeList.OfType<XmlNode>())
                {
                    _OldTailorSmallBulkOrderDeeds.Add(new OldSmallBulkOrderDeed(xmlNode));
                }
            }
        }

        private static void LoadOldSmithLargeBulkOrderDeeds(XmlDocument xmlDocument)
        {
            var mapItemXmlNodeList = xmlDocument.SelectNodes(_OldLargeSmithBulkOrderDeedMapItemXPath);

            if (mapItemXmlNodeList != null)
            {
                foreach (var mapItemXmlNode in mapItemXmlNodeList.OfType<XmlNode>())
                {
                    var mapItem = new OldLargeBulkOrderDeedMapItem(mapItemXmlNode);
                    var bulkOrderDeedXPath = String.Format(_OldLargeSmithBulkOrderDeedItemXPathFormat, mapItem.XmlName);
                    var bulkOrderDeedItemXmlNodes = xmlDocument.SelectNodes(bulkOrderDeedXPath);
                    var itemNames = new List<string>();

                    foreach (var bulkOrderDeedItemXmlNode in bulkOrderDeedItemXmlNodes.OfType<XmlNode>())
                    {
                        itemNames.Add(XmlHelper.GetNodeValue<string>(bulkOrderDeedItemXmlNode));
                    }

                    _OldSmithLargeBulkOrderDeeds.Add(new OldLargeBulkOrderDeed(mapItem, itemNames));
                }
            }
        }

        private static void LoadOldTailorLargeBulkOrderDeeds(XmlDocument xmlDocument)
        {
            var mapItemXmlNodeList = xmlDocument.SelectNodes(_OldLargeTailorBulkOrderDeedMapItemXPath);

            if (mapItemXmlNodeList != null)
            {
                foreach (var mapItemXmlNode in mapItemXmlNodeList.OfType<XmlNode>())
                {
                    var mapItem = new OldLargeBulkOrderDeedMapItem(mapItemXmlNode);
                    var bulkOrderDeedXPath = String.Format(_OldLargeTailorBulkOrderDeedItemXPathFormat, mapItem.XmlName);
                    var bulkOrderDeedItemXmlNodes = xmlDocument.SelectNodes(bulkOrderDeedXPath);
                    var itemNames = new List<string>();

                    foreach (var bulkOrderDeedItemXmlNode in bulkOrderDeedItemXmlNodes.OfType<XmlNode>())
                    {
                        itemNames.Add(XmlHelper.GetNodeValue<string>(bulkOrderDeedItemXmlNode));
                    }

                    _OldTailorLargeBulkOrderDeeds.Add(new OldLargeBulkOrderDeed(mapItem, itemNames));
                }
            }
        }

        private static void WriteNewSmithBulkOrderDeedFile()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(_XmlHeader);
            // Begin Root
            stringBuilder.AppendLine($"<{_NewDataFileRootNodeName}>");

            // Begin Small Bulk Order Deeds
            stringBuilder.AppendLine($"    <{_NewDataFileSmallNodeName}>");

            foreach (var bulkOrderDeed in _OldSmithSmallBulkOrderDeeds)
            {
                if (bulkOrderDeed.Categories.Contains(_SmithIronOnlyCategory))
                {
                    // Need to include RestrictMaterialTo with BulkOrderDeed.
                    stringBuilder.AppendLine(String.Format(_SmallBulkOrderDeedMultiLineFormat, bulkOrderDeed.Name, _TrueString, _FalseString));
                    stringBuilder.AppendLine($"            <{_RestrictedToMaterialsNodeName}>");
                    stringBuilder.AppendLine($"                {_IronMaterialNode}");
                    stringBuilder.AppendLine($"            </{_RestrictedToMaterialsNodeName}>");
                    stringBuilder.AppendLine($"        </{_BulkOrderDeedNodeName}>");
                }
                else
                {
                    // Single-line BulkOrderDeed
                    stringBuilder.AppendLine(String.Format(_SmallBulkOrderDeedSingleLineFormat, bulkOrderDeed.Name, _TrueString, _TrueString));
    }
            }

            // End Small Bulk Order Deeds
            stringBuilder.AppendLine($"    </{_NewDataFileSmallNodeName}>");

            // Begin Large Bulk Order Deeds
            stringBuilder.AppendLine($"    <{_NewDataFileLargeNodeName}>");

            foreach (var bulkOrderDeed in _OldSmithLargeBulkOrderDeeds)
            {
                stringBuilder.AppendLine(String.Format(_LargeBulkOrderDeedFirstLineFormat, bulkOrderDeed.Name));
                stringBuilder.AppendLine(_ItemsFirstLine);
                
                foreach (var itemName in bulkOrderDeed.SmallBulkOrderDeedItemNames)
                {
                    stringBuilder.AppendLine(String.Format(_ItemNodeFormat, itemName));
                }

                stringBuilder.AppendLine(_ItemsLastLine);
                stringBuilder.AppendLine(_LargeBulkOrderDeedLastLine);
            }

            // End Large Bulk Order Deeds
            stringBuilder.AppendLine($"    </{_NewDataFileLargeNodeName}>");

            // End Root
            stringBuilder.AppendLine($"</{_NewDataFileRootNodeName}>");

            File.WriteAllText(_NewSmithBulkOrderDeedFile, stringBuilder.ToString().Trim());
        }

        private static void WriteNewTailorBulkOrderDeedFile()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(_XmlHeader);
            // Begin Root
            stringBuilder.AppendLine($"<{_NewDataFileRootNodeName}>");

            // Begin Small Bulk Order Deeds
            stringBuilder.AppendLine($"    <{_NewDataFileSmallNodeName}>");

            foreach (var bulkOrderDeed in _OldTailorSmallBulkOrderDeeds)
            {
                if (bulkOrderDeed.Categories.Contains(_TailorClothOnlyCategory))
                {
                    // Need to include RestrictMaterialTo with BulkOrderDeed.
                    stringBuilder.AppendLine(String.Format(_SmallBulkOrderDeedMultiLineFormat, bulkOrderDeed.Name, _TrueString, _FalseString));
                    stringBuilder.AppendLine($"            <{_RestrictedToMaterialsNodeName}>");
                    stringBuilder.AppendLine($"                {_ClothMaterialNode}");
                    stringBuilder.AppendLine($"            </{_RestrictedToMaterialsNodeName}>");
                    stringBuilder.AppendLine($"        </{_BulkOrderDeedNodeName}>");
                }
                else
                {
                    // Need to include RestrictMaterialTo with BulkOrderDeed.
                    stringBuilder.AppendLine(String.Format(_SmallBulkOrderDeedMultiLineFormat, bulkOrderDeed.Name, _TrueString, _TrueString));
                    stringBuilder.AppendLine($"            <{_RestrictedToMaterialsNodeName}>");

                    foreach (var materialName in _TailorMaterials.Skip(1))
                    {
                        stringBuilder.AppendLine(String.Format(_MaterialNodeFormat, materialName));
                    }

                    stringBuilder.AppendLine($"            </{_RestrictedToMaterialsNodeName}>");
                    stringBuilder.AppendLine($"        </{_BulkOrderDeedNodeName}>");
                }
            }

            // End Small Bulk Order Deeds
            stringBuilder.AppendLine($"    </{_NewDataFileSmallNodeName}>");

            // Begin Large Bulk Order Deeds
            stringBuilder.AppendLine($"    <{_NewDataFileLargeNodeName}>");

            foreach (var bulkOrderDeed in _OldTailorLargeBulkOrderDeeds)
            {
                stringBuilder.AppendLine(String.Format(_LargeBulkOrderDeedFirstLineFormat, bulkOrderDeed.Name));
                stringBuilder.AppendLine(_ItemsFirstLine);

                foreach (var itemName in bulkOrderDeed.SmallBulkOrderDeedItemNames)
                {
                    stringBuilder.AppendLine(String.Format(_ItemNodeFormat, itemName));
                }

                stringBuilder.AppendLine(_ItemsLastLine);
                stringBuilder.AppendLine(_LargeBulkOrderDeedLastLine);
            }

            // End Large Bulk Order Deeds
            stringBuilder.AppendLine($"    </{_NewDataFileLargeNodeName}>");

            // End Root
            stringBuilder.AppendLine($"</{_NewDataFileRootNodeName}>");

            File.WriteAllText(_NewTailorBulkOrderDeedFile, stringBuilder.ToString().Trim());
        }
    }
}
