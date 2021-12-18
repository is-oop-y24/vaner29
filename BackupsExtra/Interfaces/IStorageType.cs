using System.Collections.Generic;
using BackupsExtra.Entities;

namespace BackupsExtra.Services
{
    public interface IStorageType
    {
        List<Storage> CreateStorages(List<JobObject> files, string path);
    }
}