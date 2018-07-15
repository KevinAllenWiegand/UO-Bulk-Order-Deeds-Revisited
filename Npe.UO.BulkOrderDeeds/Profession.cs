using System;
using System.IO;
using System.Linq;

using Npe.UO.BulkOrderDeeds.Internal;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class Profession
    {
        private const string _ProfessionDataFileName = "Profession.xml";
        private const string _BulkOrderDeedsDataFileName = "BulkOrderDeeds.xml";
        private const string _MaterialsDataFileName = "Materials.xml";
        private const string _PointRewardsDataFileName = "PointRewards.xml";
        private const string _PointTableDataFileName = "PointTable.xml";
        private const string _ProfessionNameXPath = "Profession/Name";
        private const string _BankedPointsFactorSmallXPath = "Profession/BankedPointsFactor/Small";
        private const string _BankedPointsFactorLargeXPath = "Profession/BankedPointsFactor/Large";

        private readonly string _MaterialsDataFile;

        public string Name { get; private set; }
        public double SmallBankedPointsFactor { get; private set; }
        public double LargeBankedPointsFactor { get; private set; }
        public BulkOrderDeedDefinitions BulkOrderDeedDefinitions { get; private set; }
        public BulkOrderDeedMaterials BulkOrderDeedMaterials { get; private set; }
        public PointRewards PointRewards { get; private set; }
        public PointTable PointTable { get; private set; }

        internal Profession(string path)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(path), path);

            var basePath = $"{path}\\";
            var bulkOrderDeedsDataFile = basePath + _BulkOrderDeedsDataFileName;
            var professionDataFile = basePath + _ProfessionDataFileName;
            var pointRewardsDataFile = basePath + _PointRewardsDataFileName;
            var pointTableDataFile = basePath + _PointTableDataFileName;

            _MaterialsDataFile = basePath + _MaterialsDataFileName;

            if (!File.Exists(bulkOrderDeedsDataFile))
            {
                throw new Exception($"Unable to load profession from \"{path}\", {_BulkOrderDeedsDataFileName} is missing.");
            }

            if (File.Exists(_MaterialsDataFile))
            {
                BulkOrderDeedMaterials = new BulkOrderDeedMaterials(_MaterialsDataFile);
            }

            if (!File.Exists(professionDataFile))
            {
                throw new Exception($"Unable to load profession from \"{path}\", {_ProfessionDataFileName} is missing.");
            }

            if (!File.Exists(pointRewardsDataFile))
            {
                throw new Exception($"Unable to load profession from \"{path}\", {_PointRewardsDataFileName} is missing.");
            }

            if (!File.Exists(pointTableDataFile))
            {
                throw new Exception($"Unable to load profession from \"{path}\", {_PointTableDataFileName} is missing.");
            }

            BulkOrderDeedDefinitions = new BulkOrderDeedDefinitions(bulkOrderDeedsDataFile);

            LoadProfessionData(professionDataFile);
            PointRewards = new PointRewards(pointRewardsDataFile);
            PointTable = new PointTable(pointTableDataFile);
        }

        private void LoadProfessionData(string path)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            var professionNameNode = xmlDocument.SelectSingleNode(_ProfessionNameXPath);
            var smallBankedPointsFactorNode = xmlDocument.SelectSingleNode(_BankedPointsFactorSmallXPath);
            var largeBankedPointsFactorNode = xmlDocument.SelectSingleNode(_BankedPointsFactorLargeXPath);

            Name = XmlHelper.GetNodeValue<string>(professionNameNode);
            SmallBankedPointsFactor = XmlHelper.GetNodeValue<double>(smallBankedPointsFactorNode);
            LargeBankedPointsFactor = XmlHelper.GetNodeValue<double>(largeBankedPointsFactorNode);

            var smallBulkOrderDeedDefinitions = BulkOrderDeedDefinitions.Definitions.OfType<SmallBulkOrderDeedDefinition>();

            foreach (var bulkOrderDeedDefinition in BulkOrderDeedDefinitions.Definitions.OfType<LargeBulkOrderDeedDefinition>())
            {
                bulkOrderDeedDefinition.ResolveSmallBulkOrderDeeds(smallBulkOrderDeedDefinitions, this);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
