using System;
using System.Collections.Generic;
using Backups.Entities;

namespace Backups.Services
{
    public interface IRepositoryType
    {
        IRepositoryType CreateRepository(List<JobObject> files, IStorageType storageType, string path);
        public List<Storage> GetStorages();
    }
}