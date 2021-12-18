using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class PointNumberRule : IPointRule
    {
        public PointNumberRule(uint limit)
        {
            NumberLimit = limit;
        }

        public uint NumberLimit { get; }
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints)
        {
            return restorePoints.Count < NumberLimit ? restorePoints : restorePoints.GetRange(Convert.ToInt32(restorePoints.Count - NumberLimit), Convert.ToInt32(NumberLimit));
        }
    }
}