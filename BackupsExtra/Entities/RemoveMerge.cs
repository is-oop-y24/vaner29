using System.Collections.Generic;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class RemoveMerge : IRemovalType
    {
        public void Clean(List<RestorePoint> restorePoints, IPointRule retentionRule)
        {
            throw new System.NotImplementedException();
        }
    }
}