using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Backups.Entities;
using BackupsExtra.Interfaces;
using BackupsExtra.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJob
    {
        private readonly List<RestorePoint> _restorePoints = new List<RestorePoint>();
        private string _path = @"C:\Users\PC\BackUpJob";
        private IStorageType _storageType;
        private IRepositoryType _repositoryType;
        private IPointRule _retentionRule;
        private ILogger _loggerType;
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

        public void SetRetentionRule(IPointRule retentionRule)
        {
            _retentionRule = retentionRule ?? throw new BackupsException("Invalid Rule Type");
        }

        public void SetLoggerType(ILogger loggerType)
        {
            _loggerType = loggerType ?? throw new BackupsException("Invalid Logger Type");
        }

        public void SetRemovalType(IRemovalType removalType)
        {
            _removalType = removalType ?? throw new BackupsException("Invalid Removal Type");
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
        }

        public void RecoverFilesToNewLocation(Guid restorePointId, string path)
        {
            foreach (Storage storage in GetRestorePointById(restorePointId).Rep.GetStorages())
            {
                ZipArchive archive = ZipFile.OpenRead(storage.ArchivePath);
                foreach (JobObject file in storage.GetFiles())
                {
                    File.Delete(path + Path.GetFileName(file.Name));
                    archive.Entries.FirstOrDefault(x => x.Name == Path.GetFileName(file.Name)).ExtractToFile(path);
                }
            }
        }

        public void CleanRestorePoints()
        {
            _removalType.Clean(_restorePoints, _retentionRule);
        }
    }
}