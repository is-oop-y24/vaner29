using System;
using Isu.Entities;

namespace Isu.Entities
{
    public class Lesson
    {
        public Lesson(string teacher, string dayOfWeek, string auditory, uint lessonNumber, Guid groupId)
        {
            Teacher = teacher;
            DayOfWeek = dayOfWeek;
            Auditory = auditory;
            LessonNumber = lessonNumber;
            GroupId = groupId;
        }

        public string Auditory { get; private set; }
        public string Teacher { get; private set; }
        public string DayOfWeek { get; private set; }
        public uint LessonNumber { get; private set; }
        public Guid GroupId { get; private set; }
    }
}