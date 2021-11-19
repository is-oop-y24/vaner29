using System;
using System.Collections.Generic;
using System.IO;
using Backups.Services;

namespace Backups.Entities
{
    public class SingleStorage : IStorageType
    {
        public List<Storage> CreateStorages(List<JobObject> files, Guid id, string path)
        {
            var storages = new List<Storage>();
            var curStorage = new Storage();
            foreach (JobObject curFile in files)
            {
                string name = Path.GetFileName(curFile.Name);
                name = path + @"\Storage_" + id + "_" + name;
                curStorage = curStorage.AddFileToStorage(new JobObject(name));
            }

            storages.Add(curStorage);
            return storages;
        }
    }
}