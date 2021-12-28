using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Services;

namespace BackupsExtra.Entities
{
    public class SplitStorage : IStorageType
    {
        public List<Storage> CreateStorages(List<JobObject> files, string path)
        {
            var storages = new List<Storage>();
            foreach (var curFile in files)
            {
                var id = Guid.NewGuid();
                string archivePath = path + $"{Path.DirectorySeparatorChar}" + id + ".zip";
                var curStorage = new Storage(archivePath);
                curStorage = curStorage.AddFileToStorage(curFile);
                storages.Add(curStorage);
            }

            return storages;
        }
    }
}