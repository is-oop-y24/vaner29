using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using Backups.Services;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(List<JobObject> jobObjects, IStorageType storageType, IRepositoryType repositoryType, Guid id, string path)
        {
            if (jobObjects.Count == 0)
                throw new BackupsException("No Files");
            Time = DateTime.Now;
            Rep = repositoryType.CreateRepository(jobObjects, storageType, path + $"{Path.DirectorySeparatorChar}RestorePoint_" + id);
        }

        public RestorePoint(List<JobObject> jobObjects, IStorageType storageType, IRepositoryType repositoryType, Guid id, string path, DateTime time)
        {
            if (jobObjects.Count == 0)
                throw new BackupsException("No Files");
            Time = time;
            Rep = repositoryType.CreateRepository(jobObjects, storageType, path + $"{Path.DirectorySeparatorChar}RestorePoint_" + id);
        }

        public DateTime Time { get; private set; }
        public IRepositoryType Rep { get; private set; }
    }
}