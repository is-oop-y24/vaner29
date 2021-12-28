using System.Collections.Generic;
using BackupsExtra.Entities;

namespace BackupsExtra.Interfaces
{
    public interface IRemovalType
    {
        List<RestorePoint> Clean(List<RestorePoint> restorePoints, List<RestorePoint> pointsToKeep);
    }
}