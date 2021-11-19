using System;
using System.Collections.Generic;
using Backups.Services;

namespace Backups.Entities
{
    public class Repository
    {
        private List<Storage> _storages;

        public Repository(List<JobObject> files, IStorageType storageType, Guid id, string path)
        {
            _storages = storageType.CreateStorages(files, id, path);
        }

        public List<Storage> GetStorages()
        {
            return _storages;
        }
    }
}