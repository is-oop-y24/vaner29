using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;

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
            return restorePoints.Where(point => point.Time > ExpirationDate).ToList();
        }
    }
}