using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class PointRuleHybrid : IPointRule
    {
        public PointRuleHybrid(IHybridRuleStrategy strategy, List<IPointRule> pointRules)
        {
            PointRules = pointRules;
            Strategy = strategy;
        }

        public IHybridRuleStrategy Strategy { get; }
        public List<IPointRule> PointRules { get; }
        public List<RestorePoint> RuleResult(List<RestorePoint> restorePoints)
        {
            return Strategy.RuleResult(restorePoints, PointRules);
        }
    }
}