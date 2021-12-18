using System;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class LoggerConsole : ILogger
    {
        public LoggerConsole(bool addTimeCode)
        {
            AddTimeCode = addTimeCode;
        }

        public bool AddTimeCode { get; set; }

        public void LogChanges(string change)
        {
            if (AddTimeCode)
                Console.Write("\n" + DateTime.Now + ": ");
            else Console.WriteLine(change);
        }
    }
}