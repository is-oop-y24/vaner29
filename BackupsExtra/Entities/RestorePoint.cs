using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RestorePoint
    {
        public RestorePoint(IRepositoryType repository, Guid id, string fullName)
        {
            FullName = fullName;
            Time = DateTime.Now;
            Repository = repository;
        }

        public RestorePoint(IRepositoryType repository, Guid id, string fullName, DateTime time)
        {
            FullName = fullName;
            Time = time;
            Repository = repository;
        }

        public DateTime Time { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public string FullName { get; }
        public IRepositoryType Repository { get; private set; }
    }
}