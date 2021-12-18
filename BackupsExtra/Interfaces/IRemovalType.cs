using System.Collections.Generic;
using BackupsExtra.Entities;

namespace BackupsExtra.Interfaces
{
    public interface IRemovalType
    {
        void Clean(List<RestorePoint> restorePoints);
    }
}