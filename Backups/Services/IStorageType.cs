using System;
using System.Collections.Generic;
using Backups.Entities;

namespace Backups.Services
{
    public interface IStorageType
    {
        List<Storage> CreateStorages(List<JobObject> files, Guid id, string path);
    }
}