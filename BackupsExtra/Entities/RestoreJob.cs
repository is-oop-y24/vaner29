using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BackupsExtra.Entities
{
    public class RestoreJob
    {
        public void RecoverFiles(RestorePoint restorePoint, string path)
        {
            if (path != null)
                Directory.CreateDirectory(path);
            foreach (Storage storage in restorePoint.Repository.GetStorages())
            {
                ZipArchive archive = ZipFile.OpenRead(storage.ArchivePath);
                foreach (JobObject file in storage.GetFiles())
                {
                    File.Delete(path + Path.GetFileName(file.Name));
                    if (path == null)
                        archive.Entries.FirstOrDefault(x => x.Name == Path.GetFileName(file.Name)).ExtractToFile(file.Name);
                    else
                        archive.Entries.FirstOrDefault(x => x.Name == Path.GetFileName(file.Name)).ExtractToFile(path);
                }
            }
        }
    }
}