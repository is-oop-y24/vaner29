using System.IO;
using Backups.Entities;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            FileStream file1 = File.Create(@"C:\Users\PC\trollge\man.txt");
            file1.Close();
            FileStream file2 = File.Create(@"C:\Users\PC\trollge\buys.png");
            file2.Close();
            var job = new BackupJob();
            var file1job = new JobObject(file1.Name);
            var file2job = new JobObject(file2.Name);
            job.SetRepositoryType(new RepositoryFileSystem());
            job.SetStorageType(new SplitStorage());
            job.AddFileToJobObjects(file1job);
            job.AddFileToJobObjects(file2job);
            job.CreateRestorePoint();
            job.RemoveFileFromJobObjects(file1job);
            job.CreateRestorePoint();
            job.SetStorageType(new SingleStorage());
            job.AddFileToJobObjects(file1job);
            job.CreateRestorePoint();
        }
    }
}
