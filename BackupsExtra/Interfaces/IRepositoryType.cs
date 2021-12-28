using System.Collections.Generic;
using BackupsExtra.Entities;

namespace BackupsExtra.Services
{
    public interface IRepositoryType
    {
        IRepositoryType CreateRepository(List<JobObject> files, IStorageType storageType, string path);
        public List<Storage> GetStorages();
    }
}