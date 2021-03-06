using System;
using System.Collections.Generic;
using System.IO;
using Backups.Services;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private string _path = @"C:\Users\PC\BackUpJob";
        private List<RestorePoint> _restorePoints;
        private IStorageType _storageType;
        private IRepositoryType _repositoryType;

        public BackupJob()
        {
            _restorePoints = new List<RestorePoint>();
            FileList = new List<JobObject>();
        }

        public List<JobObject> FileList { get; private set; }

        public void CreateRestorePoint()
        {
            if (FileList.Count == 0)
                throw new BackupsException("No files exist");
            _restorePoints.Add(new RestorePoint(FileList, _storageType, _repositoryType, Guid.NewGuid(), _path));
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return _restorePoints;
        }

        public void SetStorageType(IStorageType storageType)
        {
            _storageType = storageType ?? throw new BackupsException("Invalid Storage Type");
        }

        public void SetRepositoryType(IRepositoryType repositoryType)
        {
            _repositoryType = repositoryType ?? throw new BackupsException("Invalid Repository Type");
        }

        public void AddFileToJobObjects(JobObject newObject)
        {
            if (newObject == null)
                throw new BackupsException("Object is invalid");
            FileList.Add(newObject);
        }

        public void AddMultipleFilesToJobObjects(List<JobObject> newObjects)
        {
            if (newObjects.Count == 0)
                throw new BackupsException("No files exist");
            foreach (var obj in newObjects)
            {
                FileList.Add(obj);
            }
        }

        public void RemoveFileFromJobObjects(JobObject obj)
        {
            if (obj == null)
                throw new BackupsException("Object is invalid");
            if (!FileList.Contains(obj))
                throw new BackupsException("File was not added to JobOjbects");
            FileList.Remove(obj);
        }
    }
}