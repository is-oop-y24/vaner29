using System;
using System.Collections.Generic;
using System.IO;
using Backups.Services;

namespace Backups.Entities
{
    public class SplitStorage : IStorageType
    {
        public List<Storage> CreateStorages(List<JobObject> files, Guid id, string path)
        {
            var storages = new List<Storage>();
            foreach (var curFile in files)
            {
                string name = Path.GetFileName(curFile.Name);
                name = path + @"\Storage_" + id + "_" + name;
                var curStorage = new Storage();
                curStorage = curStorage.AddFileToStorage(new JobObject(name));
                storages.Add(curStorage);
            }

            return storages;
        }
    }
}