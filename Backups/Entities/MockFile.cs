namespace Backups.Entities
{
    public class MockFile
    {
        public MockFile(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}