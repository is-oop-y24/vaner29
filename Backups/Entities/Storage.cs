using System.Collections.Generic;

namespace Backups.Entities
{
    public class Storage
    {
        private List<JobObject> _files;
        public Storage()
        {
            _files = new List<JobObject>();
        }

        public Storage(List<JobObject> files)
        {
            _files = files;
        }

        public Storage AddFileToStorage(JobObject file)
        {
            _files.Add(file);
            return new Storage(_files);
        }

        public List<JobObject> GetFiles()
        {
            return _files;
        }
    }
}