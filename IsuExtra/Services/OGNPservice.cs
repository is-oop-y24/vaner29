using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class OGNPService
    {
        private List<Lesson> _curriculum = new List<Lesson>();
        private List<Group> _groups = new List<Group>();
        private List<OGNPdata> _studentsdata = new List<OGNPdata>();
        public Group AddOgnp(string name, uint maxNumberOfStudents)
        {
            if (!((((name[1] - '0') * 100) + ((name[2] - '0') * 10) + (name[3] - '0')) <= 999 && name.Length <= 4))
                throw new IsuExtraException("Invalid OGNP group name");
            _groups.Add(new Group(name, maxNumberOfStudents, Guid.NewGuid()));
            return _groups.Last();
        }

        public void RegisterStudent(Student student)
        {
            _studentsdata.Add(new OGNPdata(student.Id));
        }

        public OGNPdata FindDataByStudentId(Guid id)
        {
            return _studentsdata.FirstOrDefault(data => data.StudentId == id);
        }

        public void SignStudentToOgnp(Student student, Guid id)
        {
            if (FindDataByStudentId(student.Id) == null)
                RegisterStudent(student);
            var newData = FindDataByStudentId(student.Id).SignStudentToOgnp(id);
            _studentsdata.Remove(FindDataByStudentId(student.Id));
            _studentsdata.Add(newData);
        }

        public void RemoveStudentFromOgnp(Student student, Guid id)
        {
            var newData = FindDataByStudentId(student.Id).RemoveStudentFromOgnp(id);
            _studentsdata.Remove(FindDataByStudentId(student.Id));
            _studentsdata.Add(newData);
        }

        public List<Group> GetStudyStreamsByCourse(char num)
        {
            return _groups.Where(stream => stream.NameProperty[1] == num).ToList();
        }

        public List<Guid> GetStudentsIdsInOgnpGroup(Guid id)
        {
            return (from data in _studentsdata where data.OgnpIds.Contains(id) select data.StudentId).ToList();
        }

        public void AddLesson(string auditory, string teacher, string day, uint number, Guid group)
        {
            _curriculum.Add(new Lesson(teacher, day, auditory, number, group));
        }

        public List<Lesson> GetCurriculumByOgnpGroup(Guid id)
        {
            return _curriculum.Where(lesson => lesson.GroupId == id).ToList();
        }

        public List<Lesson> GetCurriculumByStudentId(Guid id)
        {
            var templist = new List<Lesson>();
            foreach (var ognpid in _studentsdata.Where(data => data.StudentId == id).SelectMany(data => data.OgnpIds))
            {
                templist.AddRange(GetCurriculumByOgnpGroup(ognpid));
            }

            return templist;
        }

        public Group GetGroupById(Guid id)
        {
            foreach (var group in _groups)
            {
                if (group.Id == id)
                    return group;
            }

            return null;
        }

        public bool IsStudentSignedToOgnp(Student student)
        {
            if (FindDataByStudentId(student.Id) != null)
            {
                if (FindDataByStudentId(student.Id).OgnpIds.Count != 0)
                    return true;
            }

            return false;
        }
    }
}