using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RestorePoint
    {
        public RestorePoint(List<JobObject> jobObjects, IStorageType storageType, IRepositoryType repositoryType, Guid id, string path)
        {
            if (jobObjects.Count == 0)
                throw new BackupsExtraException("No Files");
            FullName = path + $"{Path.DirectorySeparatorChar}RestorePoint_" + id;
            Time = DateTime.Now;
            Rep = repositoryType.CreateRepository(jobObjects, storageType, FullName);
        }

        public RestorePoint(List<JobObject> jobObjects, IStorageType storageType, IRepositoryType repositoryType, Guid id, string path, DateTime time)
        {
            if (jobObjects.Count == 0)
                throw new BackupsExtraException("No Files");
            FullName = path + $"{Path.DirectorySeparatorChar}RestorePoint_" + id;
            Time = time;
            Rep = repositoryType.CreateRepository(jobObjects, storageType, FullName);
        }

        public DateTime Time { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public string FullName { get; }
        public IRepositoryType Rep { get; private set; }
    }
}