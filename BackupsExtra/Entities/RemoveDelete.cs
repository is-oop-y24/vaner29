using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class RemoveDelete : IRemovalType
    {
        public void Clean(List<RestorePoint> restorePoints)
        {
            foreach (var point in restorePoints)
            {
                Directory.Delete(@"C:\Users\PC\BackUpJob\RestorePoint_" + point.Id);
            }
        }
    }
}