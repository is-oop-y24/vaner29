using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class RemoveMerge : IRemovalType
    {
        public List<RestorePoint> Clean(List<RestorePoint> restorePoints, List<RestorePoint> pointsToKeep)
        {
            foreach (RestorePoint point in restorePoints.Except(pointsToKeep))
            {
                foreach (Storage storage in point.Rep.GetStorages())
                {
                    foreach (JobObject file in storage.GetFiles().Where(file => pointsToKeep[0].Rep.GetStorages().Find(stor =>
                                 stor.GetFiles().Find(obj => obj.Name == file.Name) == null) == null))
                    {
                        using ZipArchive archive = ZipFile.Open(storage.ArchivePath, ZipArchiveMode.Update);
                        archive.CreateEntryFromFile(file.Name, Path.GetFileName(file.Name));
                    }
                }
            }

            return pointsToKeep;
        }
    }
}