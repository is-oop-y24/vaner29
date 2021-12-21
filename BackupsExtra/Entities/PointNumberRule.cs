using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;
using BackupsExtra.Tools;

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
            if (NumberLimit == 0)
                throw new BackupsExtraException("Point limit can't be 0");
            return restorePoints.Count < NumberLimit ? restorePoints : restorePoints.GetRange(Convert.ToInt32(restorePoints.Count - NumberLimit), Convert.ToInt32(NumberLimit));
        }
    }
}