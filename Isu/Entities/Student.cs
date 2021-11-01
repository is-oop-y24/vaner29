using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Entities
{
    public class Student
    {
        public Student(string name, Guid id, Guid groupId)
        {
            Id = id;
            Name = name;
            GroupId = groupId;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid GroupId { get; private set;  }

        public Student Transfer(Guid newGroupId)
        {
            return new Student(this.Name, Id, newGroupId);
        }
    }
}