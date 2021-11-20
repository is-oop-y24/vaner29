using System.Collections.Generic;
using System.IO;

namespace Backups.Entities
{
    public class Storage
    {
        private List<JobObject> _files;
        public Storage(string archivePath)
        {
            ArchivePath = archivePath;
            _files = new List<JobObject>();
        }

        public Storage(List<JobObject> files, string archivePath)
        {
            ArchivePath = archivePath;
            _files = files;
        }

        public string ArchivePath { get; }
        public Storage AddFileToStorage(JobObject file)
        {
            _files.Add(file);
            return new Storage(_files, ArchivePath);
        }

        public List<JobObject> GetFiles()
        {
            return _files;
        }
    }
}