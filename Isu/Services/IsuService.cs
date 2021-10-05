using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private Dictionary<string, Group> _groups = new ();
        private int _maxNumberOfStudents;

        public IsuService(int maxNumberOfStudents = 24)
        {
            _maxNumberOfStudents = maxNumberOfStudents;
        }

        public Group AddGroup(string name)
        {
            _groups[name] = new Group(name, _maxNumberOfStudents);
            return _groups[name];
        }

        public Student AddStudent(Group @group, string name)
        {
            return @group.AddStudent(name);
        }

        public Student GetStudent(int id)
        {
            return _groups.Values.SelectMany(groups => groups.Students).FirstOrDefault(student => student.Id == id);
        }

        public Student FindStudent(string name)
        {
            return _groups.Values.SelectMany(groups => groups.Students).FirstOrDefault(student => student.Name == name);
        }

        public List<Student> FindStudents(string groupName)
        {
            return (from groups in _groups.Values where groups.NameProperty == groupName select groups.Students).FirstOrDefault();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            List<Student> allStudents = new ();
            foreach (Group groups in _groups.Values.Where(groups => Convert.ToInt32(groups.NameProperty[2]) == courseNumber.Number))
                allStudents.AddRange(groups.Students);
            return allStudents;
        }

        public Group FindGroup(string groupName)
        {
            return _groups.Values.FirstOrDefault(groups => groups.NameProperty == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Values.Where(groups => Convert.ToInt32(groups.NameProperty[2]) == courseNumber.Number).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            newGroup.StudentTransfer(FindGroup(student.GroupName), student);
        }
    }
}