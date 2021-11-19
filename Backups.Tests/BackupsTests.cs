using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using System.Linq;
using Backups.Entities;
using Backups.Tools;

namespace Backups.Tests
{
    public class BackupTests
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
            var file1 = new JobObject("file1.txt");
            var file2 = new JobObject("file2.bmp");
            _backupJob.AddMultipleFilesToJobObjects(new List<JobObject>(){file1, file2});
            _backupJob.CreateRestorePoint();
            _backupJob.RemoveFileFromJobObjects(file2);
            _backupJob.CreateRestorePoint();
            Assert.AreEqual(2, _backupJob.GetRestorePoints().Count);
            int storageAmount = 0;
            foreach (var point in _backupJob.GetRestorePoints())
            {
                storageAmount += point.Rep.GetStorages().Count;
            }
            Assert.AreEqual(3, storageAmount);
        }

        [Test]
        public void TwoFilesSingle_Restore_DeleteFile_Restore_CheckAmount()
        {
            _backupJob.SetStorageType(new SingleStorage());
            var file1 = new JobObject("file1.txt");
            var file2 = new JobObject("file2.bmp");
            _backupJob.AddMultipleFilesToJobObjects(new List<JobObject>(){file1, file2});
            _backupJob.CreateRestorePoint();
            _backupJob.RemoveFileFromJobObjects(file2);
            _backupJob.CreateRestorePoint();
            Assert.AreEqual(2, _backupJob.GetRestorePoints().Count);
            int storageAmount = 0;
            foreach (var point in _backupJob.GetRestorePoints())
            {
                storageAmount += point.Rep.GetStorages().Count;
            }
            Assert.AreEqual(2, storageAmount);
        }

        [Test]
        public void ChangeBackupDirectory_CheckItWorkjed()
        {
            _backupJob.SetStorageType(new SingleStorage());
            var file1 = new JobObject("file1.txt");
            var file2 = new JobObject("file2.bmp");
            _backupJob.AddFileToJobObjects(file1);
            _backupJob.CreateRestorePoint();
            _backupJob.ChangeRepository(@"D:\Mono\bin");
            _backupJob.CreateRestorePoint();
            Assert.AreEqual(@"C:\Users\PC\Repository",
                Path.GetDirectoryName(_backupJob.GetRestorePoints()[0].Rep.GetStorages()[0].GetFiles()[0].Name));
            Assert.AreEqual(@"D:\Mono\bin\Repository",
                Path.GetDirectoryName(_backupJob.GetRestorePoints()[1].Rep.GetStorages()[0].GetFiles()[0].Name));
        }

        [Test]
        public void CreateRestorePointWithNoFiles_ThrowException()
        {
            Assert.Catch<BackupsException>(() =>
            {
                _backupJob.SetStorageType(new SingleStorage());
                _backupJob.CreateRestorePoint();
            });
        }
        [Test]
        public void RemoveFileWithNoFilesAdded_ThrowException()
        {
            Assert.Catch<BackupsException>(() =>
            {
                _backupJob.SetStorageType(new SingleStorage());
                var file1 = new JobObject("file1.txt");
                _backupJob.RemoveFileFromJobObjects(file1);
            });
        }
        [Test]
        public void UseInvalidStorageType_ThrowException()
        {
            Assert.Catch<BackupsException>(() =>
            {
                _backupJob.SetStorageType(null);
            });
        }
    }
}