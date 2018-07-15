using Npe.UO.BulkOrderDeeds;
using System;

namespace UO_Bulk_Order_Deeds
{
    public class ProfessionRewardSearchCriteria
    {
        public Profession Profession { get; }
        public PointReward PointReward { get; }

        public ProfessionRewardSearchCriteria(Profession profession, PointReward pointReward)
        {
            Profession = profession;
            PointReward = pointReward;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProfessionRewardSearchCriteria other))
            {
                return false;
            }

            return String.Compare(Profession.Name, other.Profession.Name, true) == 0
                && String.Compare(PointReward.Name, other.PointReward.Name, true) == 0;
        }

        public override int GetHashCode()
        {
            return Profession.Name.GetHashCode() ^ PointReward.Name.GetHashCode();
        }
    }
}
