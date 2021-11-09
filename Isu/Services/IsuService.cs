using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Isu.Entities;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Student> _students;
        private Dictionary<Guid, Group> _groups = new ();
        private List<Lesson> _curriculum = new ();
        private uint _maxNumberOfStudents;
        public IsuService(uint maxNumberOfStudents = 24)
        {
            _maxNumberOfStudents = maxNumberOfStudents;
            _students = new List<Student>();
        }

        public IReadOnlyList<Student> Students => _students;

        public Group AddGroup(string name)
        {
            if (!((((name[2] - '0') * 100) + ((name[3] - '0') * 10) + (name[4] - '0')) <= 499 && name[2] - '0' > 0 && name.Length <= 5))
                throw new IsuException("Invalid groupName");
            Guid id = Guid.NewGuid();
            _groups[id] = new Group(name, _maxNumberOfStudents, id);
            return _groups[id];
        }

        public Student AddStudent(Group group, string name)
        {
            var student = new Student(name, Guid.NewGuid(), group.Id);
            Group groupNew = group.AddStudent();
            _groups[group.Id] = groupNew;
            _students.Add(student);
            return student;
        }

        public Student GetStudent(Guid id)
        {
            return _students.FirstOrDefault(student => student.Id == id);
        }

        public Student FindStudent(Guid id)
        {
            return _students.FirstOrDefault(student => student.Id == id);
        }

        public List<Student> FindStudentsByGroup(Guid id)
        {
            return _students.Where(student => student.GroupId == id).ToList();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return (from student in _students let prefix = _groups[student.GroupId].NameProperty[2] where Convert.ToInt32(prefix) == courseNumber.Number select student).ToList();
        }

        public Group FindGroup(Guid id)
        {
            return _groups.Values.FirstOrDefault(groups => groups.Id == id);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Values.Where(groups => Convert.ToInt32(groups.NameProperty[2]) == courseNumber.Number).ToList();
        }

        public void AddLesson(string auditory, string teacher, string day, uint number, Guid group)
        {
            _curriculum.Add(new Lesson(teacher, day, auditory, number, group));
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            _groups[student.GroupId] = _groups[student.GroupId].RemoveStudent();
            Student newStudent = student.Transfer(newGroup.Id);
            _students.Remove(student);
            _students.Add(newStudent);
            _groups[newGroup.Id] = _groups[newGroup.Id].AddStudent();
        }

        public List<Lesson> GetCurriculumByGroup(Guid id)
        {
            return _curriculum.Where(lesson => lesson.GroupId == id).ToList();
        }

        public List<Lesson> GetCurriculumByStudentId(Guid id)
        {
            return GetCurriculumByGroup(FindStudent(id).GroupId);
        }
    }
}