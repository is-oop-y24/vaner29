using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class PointHybridRuleAnyOf : IHybridRuleStrategy
    {
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints, List<IPointRule> pointRules)
        {
            var ruleResult = new List<RestorePoint>();
            foreach (IPointRule rule in pointRules)
            {
                ruleResult = ruleResult.Union(rule.RuleResult(restorePoints)).ToList();
            }

            return ruleResult;
        }
    }
}