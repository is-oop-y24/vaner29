using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private string _nameProperty;
        private int _maxNumberOfStudents;

        public Group(string name, int maxNumberOfStudents)
        {
            _maxNumberOfStudents = maxNumberOfStudents;
            Students = new List<Student>();
            NameProperty = name;
        }

        public List<Student> Students { get; private set; }

        public string NameProperty
        {
            get => _nameProperty;
            private set
            {
                if (!(value.StartsWith("M3") && (((value[2] - '0') * 100) + ((value[3] - '0') * 10) + (value[4] - '0')) <= 499 && value[2] - '0' > 0 && value.Length <= 5))
                    throw new IsuException("Invalid groupName");
                _nameProperty = value;
            }
        }

        public Student AddStudent(string studentName)
        {
            if (Students.Count == _maxNumberOfStudents)
                throw new IsuException("Too Many Students In Chosen Group");
            Students.Add(new Student(studentName, NameProperty));
            return Students.Last();
        }

        public void StudentTransfer(Group oldGroup, Student student)
        {
            if (Students.Count == _maxNumberOfStudents)
                throw new IsuException("Too Many Students In Chosen Group");
            oldGroup.Students.Remove(student);
            Students.Add(student);
            student.Transfer(NameProperty);
        }
    }
}