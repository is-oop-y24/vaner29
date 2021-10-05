namespace Isu.Entities
{
    public class Student
    {
        private static int _currentId = 0;
        public Student(string name, string groupName)
        {
            _currentId++;
            Id = _currentId;
            Name = name;
            GroupName = groupName;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string GroupName { get; private set;  }

        public void Transfer(string newGroupName)
        {
            GroupName = newGroupName;
        }
    }
}