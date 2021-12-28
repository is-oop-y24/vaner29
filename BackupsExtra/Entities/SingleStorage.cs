using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Services;

namespace BackupsExtra.Entities
{
    public class SingleStorage : IStorageType
    {
        public List<Storage> CreateStorages(List<JobObject> files, string path)
        {
            var storages = new List<Storage>();
            var id = Guid.NewGuid();
            string archivePath = path + $"{Path.DirectorySeparatorChar}" + id + ".zip";
            var storage = new Storage(files, archivePath);
            storages.Add(storage);
            return storages;
        }
    }
}