using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class PointAgeRule : IPointRule
    {
        public PointAgeRule(DateTime expirationDate)
        {
            ExpirationDate = expirationDate;
        }

        public DateTime ExpirationDate { get; }
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints)
        {
            if (restorePoints.Count(point => point.Time > ExpirationDate) == 0)
                throw new BackupsExtraException("Rule can't remove all points");
            return restorePoints.Where(point => point.Time > ExpirationDate).ToList();
        }
    }
}