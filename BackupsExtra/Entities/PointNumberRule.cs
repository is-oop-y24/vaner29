using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class PointNumberRule : IPointRule
    {
        public PointNumberRule(int limit)
        {
            NumberLimit = limit;
        }

        public int NumberLimit { get; }
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints)
        {
            if (NumberLimit <= 0)
                throw new BackupsExtraException("Point limit can't be 0 or less");
            return restorePoints.Count < NumberLimit ? restorePoints : restorePoints.GetRange(restorePoints.Count - NumberLimit, NumberLimit);
        }
    }
}