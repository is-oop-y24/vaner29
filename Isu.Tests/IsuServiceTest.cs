using System.Linq;
using Isu.Entities;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group testGroup = _isuService.AddGroup("M3202");
            Student testStudent = _isuService.AddStudent(testGroup,"Anthony Blink");
            Assert.AreEqual(testStudent.GroupId, testGroup.Id);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group testGroup = _isuService.AddGroup("M3203");
                for (int i = 0; i < 30; i++)
                {
                    _isuService.AddStudent(testGroup, "Anthonille Blikiano");
                }
            });
        }

        [Test]
        [TestCase("M3502")]
        [TestCase("M34XX")]
        [TestCase("M3402a")]
        public void CreateGroupWithInvalidName_ThrowException(string name)
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(name);
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group testGroup1 = _isuService.AddGroup("M3202");
            Group testGroup2 = _isuService.AddGroup("M3299");
            Student testStudent = _isuService.AddStudent(testGroup1,"Anthony Blink");
            _isuService.ChangeStudentGroup(testStudent, testGroup2);
            _isuService.Students.Contains(testStudent);
            Assert.AreEqual(_isuService.Students.FirstOrDefault(student => student.Id == testStudent.Id).GroupId, testGroup2.Id);

        }
    }
}