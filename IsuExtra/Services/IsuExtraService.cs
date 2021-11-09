using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class IsuExtraService
    {
        private IsuService _isuService;
        private OGNPService _ognpService;

        public IsuExtraService(IsuService isuService, OGNPService ognpService)
        {
            _isuService = isuService;
            _ognpService = ognpService;
        }

        public bool CheckIfLessonsOverlap(Student student, Guid id)
        {
            foreach (var lesson1 in _ognpService.GetCurriculumByOgnpGroup(id))
            {
                foreach (var lesson2 in _isuService.GetCurriculumByStudentId(student.Id))
                {
                    if (lesson1.DayOfWeek == lesson2.DayOfWeek && lesson1.LessonNumber == lesson2.LessonNumber)
                        return true;
                }

                if (_ognpService.IsStudentSignedToOgnp(student))
                {
                    foreach (var lesson2 in _ognpService.GetCurriculumByStudentId(student.Id))
                    {
                        if (lesson1.DayOfWeek == lesson2.DayOfWeek && lesson1.LessonNumber == lesson2.LessonNumber)
                            return true;
                    }
                }
            }

            return false;
        }

        public void SignStudentToOgnp(Student student, Guid id)
        {
            if (CheckIfLessonsOverlap(student, id))
                throw new IsuExtraException("Lessons and OGNP overlap");
            if (_isuService.FindGroup(student.GroupId).NameProperty[0] == _ognpService.GetGroupById(id).NameProperty[0])
                throw new IsuExtraException("Can't sign to an OGNP from own faculty");
            _ognpService.SignStudentToOgnp(student, id);
        }

        public List<Student> GetStudentsInOgnpGroup(Guid id)
        {
            var templist = new List<Student>();
            foreach (var studid in _ognpService.GetStudentsIdsInOgnpGroup(id))
            {
                templist.Add(_isuService.FindStudent(studid));
            }

            return templist;
        }

        public List<Student> GetStudentsNotSignedForOgnps(Group group)
        {
            var templist = new List<Student>();
            foreach (var student in _isuService.FindStudentsByGroup(group.Id))
            {
                if (!_ognpService.IsStudentSignedToOgnp(student))
                    templist.Add(student);
            }

            return templist;
        }
    }
}