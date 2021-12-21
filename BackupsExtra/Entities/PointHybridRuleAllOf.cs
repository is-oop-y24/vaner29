using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class PointHybridRuleAllOf : IHybridRuleStrategy
    {
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints, List<IPointRule> pointRules)
        {
            List<RestorePoint> ruleResult = restorePoints;
            foreach (IPointRule rule in pointRules)
            {
                ruleResult = ruleResult.Intersect(rule.RuleResult(ruleResult)).ToList();
            }

            return ruleResult;
        }
    }
}