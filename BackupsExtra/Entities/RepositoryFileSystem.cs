using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BackupsExtra.Services;

namespace BackupsExtra.Entities
{
    public class RepositoryFileSystem : IRepositoryType
    {
        private List<Storage> _storages;
        public RepositoryFileSystem()
        {
        }

        private RepositoryFileSystem(List<Storage> storages)
        {
            _storages = storages;
        }

        public IReadOnlyList<Storage> Storages => _storages;

        public IRepositoryType CreateRepository(List<JobObject> files, IStorageType storageType, string path)
        {
            List<Storage> storages = storageType.CreateStorages(files, path);
            Directory.CreateDirectory(path);
            foreach (Storage storage in storages)
            {
                using (ZipArchive archive = ZipFile.Open(storage.ArchivePath, ZipArchiveMode.Update))
                {
                    foreach (JobObject file in storage.GetFiles())
                    {
                        archive.CreateEntryFromFile(file.Name, Path.GetFileName(file.Name));
                    }
                }
            }

            return new RepositoryFileSystem(storages);
        }

        public List<Storage> GetStorages()
        {
            return Storages.ToList();
        }
    }
}