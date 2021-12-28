using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class RemoveDelete : IRemovalType
    {
        public List<RestorePoint> Clean(List<RestorePoint> restorePoints, List<RestorePoint> pointsToKeep)
        {
            foreach (RestorePoint point in restorePoints.Except(pointsToKeep))
            {
                Directory.Delete(point.FullName);
            }

            return pointsToKeep;
        }
    }
}