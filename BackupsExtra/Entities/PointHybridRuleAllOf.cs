using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class PointHybridRuleAllOf : IPointRule
    {
        public PointHybridRuleAllOf(IHybridRuleStrategy strategy, List<IPointRule> pointRules)
        {
            PointRules = pointRules;
            Strategy = strategy;
        }

        public IHybridRuleStrategy Strategy { get; }
        public List<IPointRule> PointRules { get; }
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints)
        {
            var ruleResult = restorePoints;
            foreach (var rule in PointRules)
            {
                ruleResult = ruleResult.Intersect(rule.RuleResult(ruleResult)).ToList();
            }

            return ruleResult;
        }
    }
}