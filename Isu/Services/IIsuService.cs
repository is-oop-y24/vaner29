using System;
using System.Collections.Generic;
using Isu.Entities;
namespace Isu.Services
{
    public interface IIsuService
    {
        public IReadOnlyList<Student> Students { get; }
        Group AddGroup(string name);
        Student AddStudent(Group group, string name);

        Student GetStudent(Guid id);
        Student FindStudent(Guid id);
        List<Student> FindStudentsByGroup(Guid id);
        List<Student> FindStudents(CourseNumber courseNumber);

        Group FindGroup(Guid id);
        List<Group> FindGroups(CourseNumber courseNumber);
        void AddLesson(string auditory, string teacher, string day, uint number, Guid group);

        void ChangeStudentGroup(Student student, Group newGroup);
    }
}