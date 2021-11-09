using System;
using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Services;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class Tests
    {
        private OGNPService _ognpService;
        private IsuService _isuService;
        private IsuExtraService _isuExtraService;
        private Student _testStudent;
        private Group _testGroup;
        private Group _testOgnp;
        
        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
            _ognpService = new OGNPService();
            _isuExtraService = new IsuExtraService(_isuService, _ognpService);
            _testGroup = _isuService.AddGroup("M3202");
            _testStudent = _isuService.AddStudent(_testGroup, "Gleb");
            _testOgnp = _ognpService.AddOgnp("V329", 24);

        }
        [Test]
        public void AddStudentToGroupAndOgnp_CheckInData()
        {
            _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
            _ognpService.AddLesson("431", "Bom", "Mon", 2, _testOgnp.Id);
            _isuExtraService.SignStudentToOgnp(_testStudent, _testOgnp.Id);
            Assert.AreEqual(_ognpService.FindDataByStudentId(_testStudent.Id).OgnpIds[0], _testOgnp.Id);
        }
        [Test]
        public void OverlapLessonsAndOgnp()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
                _ognpService.AddLesson("431", "Bom", "Mon", 1, _testOgnp.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, _testOgnp.Id);
            });
        }
        [Test]
        public void OverlapOgnpAndOgnp()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                Group testOgnp2 = _ognpService.AddOgnp("V329", 24);
                _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
                _ognpService.AddLesson("432", "Bom", "Mon", 2, _testOgnp.Id);
                _ognpService.AddLesson("432", "Bom", "Mon", 2, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, _testOgnp.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, testOgnp2.Id);
            });
        }
        [Test]
        public void SigntoOgnpFromYourFaculty()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                Group testOgnp2 = _ognpService.AddOgnp("M329", 24);
                _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
                _ognpService.AddLesson("432", "Bom", "Mon", 2, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, testOgnp2.Id);
            });
        }
        [Test]
        public void SigntoOgnpTwice()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                Group testOgnp2 = _ognpService.AddOgnp("V329", 24);
                _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
                _ognpService.AddLesson("432", "Bom", "Mon", 2, testOgnp2.Id);
                _ognpService.AddLesson("432", "Bom", "Tue", 3, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, testOgnp2.Id);
            });
        }
        [Test]
        public void SigntoOgnpAndRemove()
        {
            Group testOgnp2 = _ognpService.AddOgnp("V329", 24);
                _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
                _ognpService.AddLesson("432", "Bom", "Mon", 2, _testOgnp.Id);
                _ognpService.AddLesson("432", "Bom", "Tue", 3, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, _testOgnp.Id);
                _ognpService.RemoveStudentFromOgnp(_testStudent, testOgnp2.Id);
            Assert.AreEqual(_ognpService.FindDataByStudentId(_testStudent.Id).OgnpIds[0], _testOgnp.Id);
        }
        [Test]
        public void SigntoOgnpAndRemoveFromDifferentOne()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                Group testOgnp2 = _ognpService.AddOgnp("V329", 24);
                _isuService.AddLesson("431", "Bim", "Mon", 1, _testGroup.Id);
                _ognpService.AddLesson("432", "Bom", "Mon", 2, _testOgnp.Id);
                _ognpService.AddLesson("432", "Bom", "Tue", 3, testOgnp2.Id);
                _isuExtraService.SignStudentToOgnp(_testStudent, _testOgnp.Id);
                _ognpService.RemoveStudentFromOgnp(_testStudent, testOgnp2.Id);
            });
        }
    }
}