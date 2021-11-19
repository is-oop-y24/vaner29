using System;
using System.Collections.Generic;
using Backups.Services;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(List<JobObject> jobObjects, IStorageType storageType, Guid id, string path)
        {
            if (jobObjects.Count == 0)
                throw new BackupsException("No Files");
            Time = DateTime.Now;
            Rep = new Repository(jobObjects, storageType, id, path);
        }

        public DateTime Time { get; private set; }
        public Repository Rep { get; private set; }
    }
}