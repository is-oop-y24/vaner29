using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using BackupsExtra.Entities;
using BackupsExtra.Tools;
using Newtonsoft.Json;
namespace BackupsExtra.Manager
{
    public class BackupJobManager
    {
        private readonly List<BackupJob> _backupJobs;

        public BackupJobManager(string path)
        {
            _backupJobs = JsonConvert.DeserializeObject<List<BackupJob>>(File.ReadAllText(path));
        }

        public void AddBackupJob(BackupJob job)
        {
            if (job == null)
                throw new BackupsException("Can't add a null backup job to manager");
            _backupJobs.Add(job);
        }

        public BackupJob GetJobById(Guid id)
        {
            if (_backupJobs.FirstOrDefault(x => x.Id == id) == null)
                throw new BackupsException("Can't find a backup job with this Id");
            return _backupJobs.FirstOrDefault(x => x.Id == id);
        }

        public void SerializeJobs(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(_backupJobs));
        }
    }
}