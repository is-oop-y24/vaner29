using System;
using System.IO;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class LoggerFile : ILogger
    {
        public LoggerFile(string path)
        {
            LogPath = path;
        }

        public bool AddTimeCode { get; set; }
        public string LogPath { get; }

        public void LogChanges(string change)
        {
            using StreamWriter sw = File.AppendText(LogPath);
            if (AddTimeCode)
                sw.Write("\n" + DateTime.Now + ": ");
            sw.WriteLine(change);
        }
    }
}