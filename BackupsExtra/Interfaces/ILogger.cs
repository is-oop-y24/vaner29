namespace BackupsExtra.Interfaces
{
    public interface ILogger
    {
        public bool AddTimeCode { get; set; }
        void LogChanges(string change);
    }
}