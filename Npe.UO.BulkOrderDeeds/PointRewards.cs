using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class PointRewards
    {
        private const string _RewardXPath = "PointRewards/Reward";

        private readonly List<PointReward> _Rewards;

        public IEnumerable<PointReward> Rewards => _Rewards.AsReadOnly();

        internal PointRewards(string path)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            var rewardNodes = xmlDocument.SelectNodes(_RewardXPath);

            if (rewardNodes == null)
            {
                throw new Exception($"Unable to find any defined rewards in {path}.");
            }

            _Rewards = new List<PointReward>();

            foreach (var rewardNode in rewardNodes.OfType<XmlNode>())
            {
                _Rewards.Add(new PointReward(rewardNode));
            }
        }
    }
}
