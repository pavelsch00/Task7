using System;
using System.Collections.Generic;
using System.Linq;
using Epam_Task7.Enums;
using Microsoft.Office.Interop.Excel;

namespace Epam_Task7.Reports
{
    /// <summary>
    /// Class generation reports.
    /// </summary>
    public class GenerationReport
    {
        /// <summary>
        /// The constructor initializes the GenerationReport.
        /// </summary>
        public GenerationReport()
        {
            StudentsDataContext = new StudentsDataContext();
        }

        /// <summary>
        /// The property stores information about StudentsDataContext.
        /// </summary>
        public StudentsDataContext StudentsDataContext { get; set; }

        /// <summary>
        /// Method saving and generation specialty result by session.
        /// </summary>
        /// <param name="pathToFile">Path to file.</param>
        /// <param name="sortableSheet">Sorted table number.</param>
        /// <param name="sortOrder">Sort order.</param>
        public void GenerationSpecialtyResultBySession(int sessionNumber, string pathToFile, int sortableSheet, SortOrder sortOrder)
        {
            Application application = new Application();
            Workbook workBook = application.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Session";
            workSheet.Cells[1, "B"] = "Specialty";
            workSheet.Cells[1, "C"] = "Average Mark";

            int i = 2;

            List<int> temp = StudentsDataContext.StudentResults.Select(item => item.SessionEducationalSubjects.Sessions.Groups.SpecialtyId.Value).Distinct().ToList();

            foreach (StudentResults item in StudentsDataContext.StudentResults)
            {
                if (temp.Where(obj => obj == item?.SessionEducationalSubjects?.Sessions.Groups.SpecialtyId).Count() == 1 &&
                    item?.SessionEducationalSubjects?.Sessions.SessionNumber == sessionNumber)
                {
                    var examList = StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList();

                    workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Sessions.SessionNumber;
                    workSheet.Cells[i, "B"] = item.SessionEducationalSubjects.Sessions.Groups.Specialtys.Name;
                    workSheet.Cells[i, "C"] = GetStudentMarkForSpecialty(examList,
                        item.SessionEducationalSubjects.Sessions.SessionNumber,
                        item.SessionEducationalSubjects.Sessions.Groups.SpecialtyId.Value).Average();
                    i++;
                    temp.Add(item.SessionEducationalSubjects.SessionId.Value);
                }
            }

            SortSheet(workSheet, i, sortableSheet, (XlSortOrder)sortOrder);

            try
            {
                workBook.Close(true, $"{Environment.CurrentDirectory}" + pathToFile);
                application.Quit();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid path to file.");
            }
        }

        /// <summary>
        /// Method generation session result by examinator.
        /// </summary>
        /// <param name="pathToFile">Path to file.</param>
        /// <param name="sortableSheet">Sorted table number.</param>
        /// <param name="sortOrder">Sort order.</param>
        public void GenerationSessionResultByExaminator(int sessionNumber, string pathToFile, int sortableSheet, SortOrder sortOrder)
        {
            Application application = new Application();
            Workbook workBook = application.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Seeion";
            workSheet.Cells[1, "B"] = "Examainer";
            workSheet.Cells[1, "C"] = "Average Mark";

            int i = 2;

            List<int> temp = StudentsDataContext.StudentResults.Select(item => item.SessionEducationalSubjects.EducationalSubjects.ExamainersId.Value).Distinct().ToList();

            foreach (StudentResults item in StudentsDataContext.StudentResults)
            {
                if (temp.Where(obj => obj == item?.SessionEducationalSubjects?.EducationalSubjects.ExamainersId.Value).Count() == 1 &&
                    item?.SessionEducationalSubjects?.Sessions.SessionNumber == sessionNumber)
                {
                    var examList = StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList();

                    var markByExamainr = GetStudentMarkForExamainer(examList,
                        item.SessionEducationalSubjects.Sessions.SessionNumber,
                        item.SessionEducationalSubjects.EducationalSubjects.ExamainersId.Value);

                    if(markByExamainr.Count != 0)
                    {
                        workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Sessions.SessionNumber;
                        workSheet.Cells[i, "B"] = item.SessionEducationalSubjects.EducationalSubjects.Examiners.FullName;
                        workSheet.Cells[i, "C"] = markByExamainr.Average();
                        i++;
                    }
                    temp.Add(item.SessionEducationalSubjects.EducationalSubjects.ExamainersId.Value);
                }
            }

            SortSheet(workSheet, i, sortableSheet, (XlSortOrder)sortOrder);

            try
            {
                workBook.Close(true, $"{Environment.CurrentDirectory}" + pathToFile);
                application.Quit();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid path to file.");
            }
        }

        /// <summary>
        /// Method generation average result student By Year.
        /// </summary>
        /// <param name="pathToFile">Path to file.</param>
        /// <param name="sortableSheet">Sorted table number.</param>
        /// <param name="sortOrder">Sort order.</param>
        public void GenerationAverageResultStudentByYear(string pathToFile, int sortableSheet, SortOrder sortOrder)
        {
            Application application = new Application();
            Workbook workBook = application.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Year";
            workSheet.Cells[1, "B"] = "EducationName";
            workSheet.Cells[1, "C"] = "Average Mark";
            int i = 2;

            List<DateTime> uniqueDate = StudentsDataContext.StudentResults.Select(item => item.SessionEducationalSubjects.Date).Distinct().ToList();
            List<int> uniqueEducationSubectId = StudentsDataContext.StudentResults.Select(item => item.
                SessionEducationalSubjects.EducationalSubjectId.Value).Distinct().ToList();
            int count = 0;
            foreach (StudentResults item in StudentsDataContext.StudentResults)
            {
                if (uniqueDate.Where(obj => obj == item?.SessionEducationalSubjects.Date).Count() == 1)
                {
                    var examList = StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList();
                    List<double> markByYear = GetStudentMarkForYear(examList,
                        item.SessionEducationalSubjects.Date,
                        item.SessionEducationalSubjects.EducationalSubjects.SubjectName);
                    if(markByYear.Count != 0)
                    {
                        workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Date.Year;
                        workSheet.Cells[i, "B"] = item.SessionEducationalSubjects.EducationalSubjects.SubjectName;
                        workSheet.Cells[i, "C"] = markByYear.Average();
                        i++;
                    }
                    uniqueEducationSubectId.Add(item.SessionEducationalSubjects.EducationalSubjectId.Value);
                    uniqueDate.Add(item.SessionEducationalSubjects.Date);
                    count++;
                }

            }

            SortSheet(workSheet, i, sortableSheet, (XlSortOrder)sortOrder);

            try
            {
                workBook.Close(true, $"{Environment.CurrentDirectory}" + pathToFile);
                application.Quit();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid path to file.");
            }
        }

        /// <summary>
        /// Method sort sheet.
        /// </summary>
        /// <param name="workSheet">Work Sheet.</param>
        /// <param name="maxLine">Max Line.</param>
        /// <param name="sortableSheet">Sortable Sheet.</param>
        /// <param name="xlSortOrder">Sort Order.</param>
        private static void SortSheet(Worksheet workSheet, int maxLine, int sortableSheet, XlSortOrder xlSortOrder)
        {
            var rngSort = workSheet.get_Range("A1", $"F{maxLine}");
            rngSort.Sort(rngSort.Columns[sortableSheet, Type.Missing], xlSortOrder,
            null, Type.Missing, XlSortOrder.xlAscending,
            Type.Missing, XlSortOrder.xlAscending,
            XlYesNoGuess.xlYes, Type.Missing, Type.Missing,
            XlSortOrientation.xlSortColumns);
        }

        /// <summary>
        /// Method get student mark for specialty.
        /// </summary>
        /// <param name="listStudentResults">List student results.</param>
        /// <param name="sessionNumber">Session Number.</param>
        /// <param name="specialtyId">Specialty Id.</param>
        /// <returns>Student mark list.</returns>
        private static List<double> GetStudentMarkForSpecialty(List<StudentResults> listStudentResults, int sessionNumber, int specialtyId)
        {
            return listStudentResults.Where(item => item.SessionEducationalSubjects.Sessions.SessionNumber == sessionNumber
            && item.SessionEducationalSubjects.Sessions.Groups.SpecialtyId == specialtyId)
                .Select(item => double.Parse(item.Mark)).ToList();
        }

        /// <summary>
        /// Method get student mark for examainer.
        /// </summary>
        /// <param name="listStudentResults">List student results.</param>
        /// <param name="sessionNumber">Session Number.</param>
        /// <param name="examainersId">Examainer Id.</param>
        /// <returns>Student mark list.</returns>
        private static List<double> GetStudentMarkForExamainer(List<StudentResults> listStudentResults, int sessionNumber, int examainersId)
        {
            return listStudentResults.Where(item => item.SessionEducationalSubjects.Sessions.SessionNumber == sessionNumber
            && item.SessionEducationalSubjects.EducationalSubjects.ExamainersId == examainersId)
                .Select(item => double.Parse(item.Mark)).ToList();
        }

        /// <summary>
        /// Method get student mark for Year.
        /// </summary>
        /// <param name="listStudentResults">List student results.</param>
        /// <param name="date">Exam date.</param>
        /// <param name="educationalSubjectName">Educational subject name.</param>
        /// <returns>Student mark list.</returns>
        private static List<double> GetStudentMarkForYear(List<StudentResults> listStudentResults, DateTime date, string educationalSubjectName)
        {
            return listStudentResults.Where(item => item.SessionEducationalSubjects.Date == date
            && item.SessionEducationalSubjects.EducationalSubjects.SubjectName == educationalSubjectName)
                .Select(item => double.Parse(item.Mark)).ToList();
        }
    }
}
