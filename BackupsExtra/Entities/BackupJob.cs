using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using BackupsExtra.Interfaces;
using BackupsExtra.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJob
    {
        private List<RestorePoint> _restorePoints = new List<RestorePoint>();
        private string _path;
        private IStorageType _storageType;
        private IRepositoryType _repositoryType;
        private IPointRule _retentionRule;
        private ILogger _loggerType;
        private IRemovalType _removalType;
        private List<JobObject> _fileList;

        public BackupJob(string path, IStorageType storageType, IRepositoryType repositoryType, IPointRule retentionRule, ILogger loggerType, IRemovalType removalType)
        {
            _path = path;
            _storageType = storageType;
            _repositoryType = repositoryType;
            _retentionRule = retentionRule;
            _loggerType = loggerType;
            _removalType = removalType;
            _fileList = new List<JobObject>();
        }

        public Guid Id { get; } = Guid.NewGuid();
        public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.OrderBy(x => x.Time).ToList();
        public void CreateRestorePoint()
        {
            if (_fileList.Count == 0)
                throw new BackupsExtraException("No files exist");
            var id = Guid.NewGuid();
            string path = _path + $"{Path.DirectorySeparatorChar}RestorePoint_" + id;
            IRepositoryType repository = _repositoryType.CreateRepository(_fileList, _storageType, path);
            _restorePoints.Add(new RestorePoint(repository, id, _path));
            _loggerType.LogChanges("Restore Point " + id + " Created");
        }

        public void SetStorageType(IStorageType storageType)
        {
            _storageType = storageType ?? throw new BackupsExtraException("Invalid Storage Type");
            _loggerType.LogChanges("Storage Type Changed");
        }

        public void SetRepositoryType(IRepositoryType repositoryType)
        {
            _repositoryType = repositoryType ?? throw new BackupsExtraException("Invalid Repository Type");
            _loggerType.LogChanges("Repository Type Changed");
        }

        public void SetRetentionRule(IPointRule retentionRule)
        {
            _retentionRule = retentionRule ?? throw new BackupsExtraException("Invalid Rule Type");
            _loggerType.LogChanges("Retention Rule Type Changed");
        }

        public void SetLoggerType(ILogger loggerType)
        {
            _loggerType = loggerType ?? throw new BackupsExtraException("Invalid Logger Type");
            _loggerType.LogChanges("Logger Type Changed");
        }

        public void SetRemovalType(IRemovalType removalType)
        {
            _removalType = removalType ?? throw new BackupsExtraException("Invalid Removal Type");
            _loggerType.LogChanges("Point Removal Type Changed");
        }

        public void AddFileToJobObjects(JobObject newObject)
        {
            if (newObject == null)
                throw new BackupsExtraException("Object is invalid");
            _fileList.Add(newObject);
        }

        public void AddMultipleFilesToJobObjects(List<JobObject> newObjects)
        {
            if (newObjects.Count == 0)
                throw new BackupsExtraException("No files exist");
            foreach (JobObject obj in newObjects)
            {
                _fileList.Add(obj);
            }
        }

        public void RemoveFileFromJobObjects(JobObject obj)
        {
            if (obj == null)
                throw new BackupsExtraException("Object is invalid");
            if (!_fileList.Contains(obj))
                throw new BackupsExtraException("File was not added to JobOjbects");
            _fileList.Remove(obj);
        }

        public RestorePoint GetRestorePointById(Guid id)
        {
            return _restorePoints.FirstOrDefault(point => point.Id == id);
        }

        public void CleanRestorePoints()
        {
            _restorePoints = _removalType.Clean(RestorePoints.ToList(), _retentionRule.RuleResult(RestorePoints.ToList())).ToList();
            _loggerType.LogChanges("Old restore points were removed");
        }
    }
}