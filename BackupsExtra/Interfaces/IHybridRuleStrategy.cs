using System.Collections.Generic;
using BackupsExtra.Entities;

namespace BackupsExtra.Interfaces
{
    public interface IHybridRuleStrategy
    {
        List<RestorePoint> RuleResult(List<RestorePoint> restorePoints, List<IPointRule> pointRules);
    }
}