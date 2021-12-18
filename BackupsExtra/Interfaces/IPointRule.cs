using System.Collections.Generic;
using BackupsExtra.Entities;

namespace BackupsExtra.Interfaces
{
    public interface IPointRule
    {
        List<RestorePoint> RuleResult(List<RestorePoint> restorePoints);
    }
}