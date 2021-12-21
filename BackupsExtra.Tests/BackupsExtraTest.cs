using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Entities;
using BackupsExtra.Tools;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTests
    {
        private BackupJob _backupJob;

        [SetUp]
        public void Setup()
        {
            _backupJob = new BackupJob();
        }

        [Test]
        public void TwoFilesSplit_Restore_DeleteFile_Restore_CheckAmount()
        {
            _backupJob.SetStorageType(new SplitStorage());
            _backupJob.SetRepositoryType(new RepositoryNoFiles());
            var file1 = new JobObject("file1.txt");
            var file2 = new JobObject("file2.bmp");
            _backupJob.AddMultipleFilesToJobObjects(new List<JobObject>(){file1, file2});
            _backupJob.CreateRestorePoint();
            _backupJob.RemoveFileFromJobObjects(file2);
            _backupJob.CreateRestorePoint();
            int storageAmount = _backupJob.GetRestorePoints().Sum(point => point.Rep.GetStorages().Count);
            Assert.AreEqual(2, _backupJob.GetRestorePoints().Count);
            Assert.AreEqual(3, storageAmount);
        }

        [Test]
        public void TwoFilesSingle_Restore_DeleteFile_Restore_CheckAmount()
        {
            _backupJob.SetStorageType(new SingleStorage());
            _backupJob.SetRepositoryType(new RepositoryNoFiles());
            var file1 = new JobObject("file1.txt");
            var file2 = new JobObject("file2.bmp");
            _backupJob.AddMultipleFilesToJobObjects(new List<JobObject>(){file1, file2});
            _backupJob.CreateRestorePoint();
            _backupJob.RemoveFileFromJobObjects(file2);
            _backupJob.CreateRestorePoint();
            Assert.AreEqual(2, _backupJob.GetRestorePoints().Count);
            int storageAmount = _backupJob.GetRestorePoints().Sum(point => point.Rep.GetStorages().Count);
            Assert.AreEqual(2, storageAmount);
        }

        [Test]
        public void CreateRestorePointWithNoFiles_ThrowException()
        {
            _backupJob.SetStorageType(new SingleStorage());
            Assert.Catch<BackupsExtraException>(() =>
            {
                _backupJob.CreateRestorePoint();
            });
        }
        [Test]
        public void RemoveFileWithNoFilesAdded_ThrowException()
        {
            _backupJob.SetStorageType(new SingleStorage());
            var file1 = new JobObject("file1.txt");
            Assert.Catch<BackupsExtraException>(() =>
            {
                _backupJob.RemoveFileFromJobObjects(file1);
            });
        }
        [Test]
        public void UseInvalidStorageType_ThrowException()
        {
            Assert.Catch<BackupsExtraException>(() =>
            {
                _backupJob.SetStorageType(null);
            });
        }
    }
}