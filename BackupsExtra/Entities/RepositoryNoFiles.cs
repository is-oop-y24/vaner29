using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Services;

namespace BackupsExtra.Entities
{
    public class RepositoryNoFiles : IRepositoryType
    {
        private List<Storage> _storages;
        public RepositoryNoFiles()
        {
        }

        private RepositoryNoFiles(List<Storage> storages)
        {
            _storages = storages;
        }

        public IReadOnlyList<Storage> Storages => _storages;
        public IRepositoryType CreateRepository(List<JobObject> files, IStorageType storageType, string path)
        {
            _storages = storageType.CreateStorages(files, path);
            return new RepositoryNoFiles(storageType.CreateStorages(files, path));
        }

        public List<Storage> GetStorages()
        {
            return Storages.ToList();
        }
    }
}