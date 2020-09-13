using System;
using System.Collections.Generic;
using System.Linq;
using Epam_Task7;
using Epam_Task7.CRUD;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Epam_Task7_Test.CRUD_Tests
{
    /// <summary>
    /// Class for testing class crud.
    /// </summary>
    [TestClass]
    public class BasicMethodDbTest
    {
        /// <summary>
        /// The method tests the method add and delete student.
        /// </summary>
        [TestMethod]
        public void Add_AddStudentToDataBase_AddStudent()
        {
            BasicMethodDb<Students> studentDBContext = new BasicMethodDb<Students>();
            var student = new List<Students>()
            {
                new Students
                {
                    FullName = "Saladuhin Pavel Viktorovich",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1999, 01, 25),
                    GroupId = 1
                }
            };

            studentDBContext.Create(student);

            List<Students> resultList = studentDBContext.Read();
            studentDBContext.Delete(resultList.Last().Id);

            Assert.AreEqual(resultList.Last().Id, student[0].Id);
        }
        
        /// <summary>
        /// The method tests the method add and delete group.
        /// </summary>
        [TestMethod]
        public void Add_AddAndDeleteGroupToDataBase_AddAndDaleteGroup()
        {
            var groupDBContext = new BasicMethodDb<Groups>();
            var group = new List<Groups>()
            {
                new Groups()
                {
                    Name = "PM-22"
                }
            };

            groupDBContext.Create(group);

            List<Groups> resultList = groupDBContext.Read();
            groupDBContext.Delete(groupDBContext.Read().Last().Id);

            Assert.AreEqual(resultList.Last().Id, group[0].Id);
        }

        /// <summary>
        /// The method tests the method add and delete educationalSubject.
        /// </summary>
        [TestMethod]
        public void Add_AddAndDeleteEducationalSubjectToDataBase_AddAndDaleteEducationalSubject()
        {
            var educationalSubjectsDBContext = new BasicMethodDb<EducationalSubjects>();
            var educationalSubject = new List<EducationalSubjects>()
            {
                new EducationalSubjects()
                {
                    SubjectName = "Drawing",
                    SubjectType = "Exam"
                }
            };

            educationalSubjectsDBContext.Create(educationalSubject);

            List<EducationalSubjects> resultList = educationalSubjectsDBContext.Read();
            educationalSubjectsDBContext.Delete(educationalSubjectsDBContext.Read().Last().Id);

            Assert.AreEqual(resultList.Last().Id, educationalSubject[0].Id);
        }

        /// <summary>
        /// The method tests the method add and delete session.
        /// </summary>
        [TestMethod]
        public void Add_AddAndDeleteSessionToDataBase_AddAndDaleteSession()
        {
            var sessionDBContext = new BasicMethodDb<Sessions>();
            var session = new List<Sessions>()
            {
                new Sessions()
                {
                    SessionNumber = 1,
                    GroupId = 5
                }
            };

            sessionDBContext.Create(session);

            List<Sessions> resultList = sessionDBContext.Read();
            sessionDBContext.Delete(sessionDBContext.Read().Last().Id);

            Assert.AreEqual(resultList.Last().Id, session[0].Id);
        }
        
        /// <summary>
        /// The method tests the method add and delete sessionEducationalSubject.
        /// </summary>
        [TestMethod]
        public void Add_AddAndDeleteSessionEducationalSubjectToDataBase_AddAndDeleteSessionEducationalSubject()
        {
            var sessionEducationalSubjectDBContext = new BasicMethodDb<SessionEducationalSubjects>();
            var sessionEducationalSubject = new List<SessionEducationalSubjects>()
            {
                new SessionEducationalSubjects()
                {
                    Date = new DateTime(2020, 08, 15),
                    EducationalSubjectId = 3,
                    SessionId = 4
                }
            };

            sessionEducationalSubjectDBContext.Create(sessionEducationalSubject);

            List<SessionEducationalSubjects> resultList = sessionEducationalSubjectDBContext.Read();
            sessionEducationalSubjectDBContext.Delete(sessionEducationalSubjectDBContext.Read().Last().Id);

            Assert.AreEqual(resultList.Last().Id, sessionEducationalSubject[0].Id);
        }
        
        /// <summary>
        /// The method tests the method add and delete studentResult.
        /// </summary>
        [TestMethod]
        public void Add_AddAndDeleteStudentResultToDataBase_AddAndDeleteStudentResult()
        {
            var studentResultsDBContext = new BasicMethodDb<StudentResults>();
            var studentResult = new List<StudentResults>()
            {
                new StudentResults()
                {
                    StudentId = 8,
                    Mark = "10",
                    SessionEducationalSubjectId = 5
                }
            };

            studentResultsDBContext.Create(studentResult);

            List<StudentResults> resultList = studentResultsDBContext.Read();
            studentResultsDBContext.Delete(studentResultsDBContext.Read().Last().Id);

            Assert.AreEqual(resultList.Last().Id, studentResult[0].Id);
        }
        
        /// <summary>
        /// The method tests the method Change.
        /// </summary>
        [TestMethod]
        public void Chenge_Student_StudentChenges()
        {
            var studentsDBContext = new BasicMethodDb<Students>();
            var student = new List<Students>()
            {
                new Students()
                {
                    FullName = "Saladuhin Pavel Viktorovich",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1999, 01, 25),
                    GroupId = 1
                }
            };

            var newStudent = new Students()
            {
                FullName = "Saladuhin Pavel Viktorovich",
                Gender = "Woman",
                DateOfBirth = new DateTime(2000, 11, 15),
                GroupId = 2
            };

            studentsDBContext.Create(student);
            studentsDBContext.Update(studentsDBContext.Read().Last().Id, newStudent);
            Students result = studentsDBContext.Read().Last();
            studentsDBContext.Delete(studentsDBContext.Read().Last().Id);

            Assert.AreEqual(result.FullName, newStudent.FullName);
        }
        
        /// <summary>
        /// The method tests the method Delete object.
        /// </summary>
        [TestMethod]
        public void Delete_DeleteObject_DeleteObject()
        {
            BasicMethodDb<Students> studentDBContext = new BasicMethodDb<Students>();
            var student = new List<Students>()
            {
                new Students
                {
                    FullName = "Ikipnik Pavel Viktorovich",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1999, 01, 25),
                    GroupId = 1
                }
            };

            studentDBContext.Create(student);

            List<Students> resultList = studentDBContext.Read();
            studentDBContext.Delete(resultList.Last().Id);

            Assert.AreEqual(resultList.Last().Id, student[0].Id);
        }
    }
}
