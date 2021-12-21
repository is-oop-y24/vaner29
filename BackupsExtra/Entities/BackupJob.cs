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
        private string _path = @"C:\Users\PC\BackUpJob";
        private IStorageType _storageType;
        private IRepositoryType _repositoryType;
        private IPointRule _retentionRule;
        private ILogger _loggerType = new LoggerConsole(true);
        private IRemovalType _removalType;

        public BackupJob()
        {
            FileList = new List<JobObject>();
        }

        public Guid Id { get; } = Guid.NewGuid();
        public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.OrderBy(x => x.Time).ToList();

        public List<JobObject> FileList { get; private set; }

        public void CreateRestorePoint()
        {
            if (FileList.Count == 0)
                throw new BackupsExtraException("No files exist");
            _restorePoints.Add(new RestorePoint(FileList, _storageType, _repositoryType, Guid.NewGuid(), _path));
            _loggerType.LogChanges("Restore Point Created");
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return _restorePoints;
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
            FileList.Add(newObject);
        }

        public void AddMultipleFilesToJobObjects(List<JobObject> newObjects)
        {
            if (newObjects.Count == 0)
                throw new BackupsExtraException("No files exist");
            foreach (JobObject obj in newObjects)
            {
                FileList.Add(obj);
            }
        }

        public void RemoveFileFromJobObjects(JobObject obj)
        {
            if (obj == null)
                throw new BackupsExtraException("Object is invalid");
            if (!FileList.Contains(obj))
                throw new BackupsExtraException("File was not added to JobOjbects");
            FileList.Remove(obj);
        }

        public RestorePoint GetRestorePointById(Guid id)
        {
            return _restorePoints.FirstOrDefault(point => point.Id == id);
        }

        public void RecoverFilesToPreviousLocation(Guid restorePointId)
        {
            foreach (Storage storage in GetRestorePointById(restorePointId).Rep.GetStorages())
            {
                ZipArchive archive = ZipFile.OpenRead(storage.ArchivePath);
                foreach (JobObject file in storage.GetFiles())
                {
                    File.Delete(file.Name);
                    archive.Entries.FirstOrDefault(x => x.Name == Path.GetFileName(file.Name)).ExtractToFile(file.Name);
                }
            }

            _loggerType.LogChanges("Files from restore point were recovered");
        }

        public void CleanRestorePoints()
        {
            _restorePoints = _removalType.Clean(RestorePoints.ToList(), _retentionRule.RuleResult(RestorePoints.ToList())).ToList();
            _loggerType.LogChanges("Old restore points were removed");
        }
    }
}