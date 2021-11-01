using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private string _nameProperty;
        private uint _maxNumberOfStudents;

        public Group(string name, uint maxNumberOfStudents, Guid groupId)
        {
            _maxNumberOfStudents = maxNumberOfStudents;
            Id = groupId;
            StudentCount = 0;
            NameProperty = name;
        }

        public Group(string name, uint maxNumberOfStudents, Guid groupId, uint studentCount)
        {
            _maxNumberOfStudents = maxNumberOfStudents;
            Id = groupId;
            StudentCount = studentCount;
            NameProperty = name;
        }

        public Guid Id { get; private set; }

        public string NameProperty
        {
            get => _nameProperty;
            private set
            {
                _nameProperty = value;
            }
        }

        private uint StudentCount { get; set; }

        public Group AddStudent()
        {
            if (StudentCount == _maxNumberOfStudents)
                throw new IsuException("Too Many Students In Chosen Group");
            return new Group(NameProperty, _maxNumberOfStudents, Id, StudentCount++);
        }

        public Group RemoveStudent()
        {
            return new Group(NameProperty, _maxNumberOfStudents, Id, StudentCount--);
        }
    }
}