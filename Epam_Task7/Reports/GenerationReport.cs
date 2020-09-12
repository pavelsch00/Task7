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
        /// <param name="name">Groups name.</param>
        public GenerationReport()
        {
            StudentsDataContext = new StudentsDataContext();
        }

        /// <summary>
        /// The property stores information about StudentDBContext.
        /// </summary>
        public StudentsDataContext StudentsDataContext { get; set; }

        /// <summary>
        /// Method saving in xlsx format file of Sessions results for Sessions group in the form of a table.
        /// </summary>
        /// <param name="pathToFile">Path to file.</param>
        /// <param name="sortableSheet">Sorted table number.</param>
        /// <param name="sortOrder">Sort order.</param>
        public void GenerationSpecialtyResultBySession(string pathToFile, int sortableSheet, SortOrder sortOrder)
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
                if (temp.Where(obj => obj == item?.SessionEducationalSubjects?.Sessions.Groups.SpecialtyId).Count() == 1)
                {
                    var examList = StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList();
                    workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Sessions.SessionNumber;
                    workSheet.Cells[i, "B"] = item.SessionEducationalSubjects.Sessions.Groups.Specialtys.Name;
                    workSheet.Cells[i, "C"] = GetStudentMarkForSpecialty(examList,
                        item.SessionEducationalSubjects.SessionId.Value,
                        item.SessionEducationalSubjects.Sessions.Groups.SpecialtyId.Value).Average();
                    i++;
                    temp.Add(item.SessionEducationalSubjects.Sessions.Groups.SpecialtyId.Value);
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
        /// Method for each session, display the xlsx pivot table with average / minimum / maximum score for each group.
        /// </summary>
        /// <param name="pathToFile">Path to file.</param>
        /// <param name="sortableSheet">Sorted table number.</param>
        /// <param name="sortOrder">Sort order.</param>
        public void GenerationResultSummaryTableByGroup(string pathToFile, int sortableSheet, SortOrder sortOrder)
        {
            Application application = new Application();
            Workbook workBook = application.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Sessions";
            workSheet.Cells[1, "B"] = "Groups";
            workSheet.Cells[1, "C"] = "Average Mark";
            workSheet.Cells[1, "D"] = "Min Mark";
            workSheet.Cells[1, "E"] = "Max Mark";

            int i = 2;

            List<int> temp = StudentsDataContext.StudentResults.Select(item => item.SessionEducationalSubjects.SessionId.Value).Distinct().ToList();

            foreach (StudentResults item in StudentsDataContext.StudentResults)
            {
                if (temp.Where(obj => obj == item?.SessionEducationalSubjects?.SessionId).Select(obj => obj).Count() == 1)
                {
                    var examList = StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList();

                    workSheet.Cells[i, "A"] = item?.SessionEducationalSubjects?.Sessions?.SessionNumber;
                    workSheet.Cells[i, "B"] = item?.SessionEducationalSubjects?.Sessions?.Groups.Name;

                    workSheet.Cells[i, "C"] = GetStudentMarkForSpecialty(examList,
                        item.SessionEducationalSubjects.SessionId.Value,
                        item.SessionEducationalSubjects.Sessions.GroupId.Value).Average();

                    workSheet.Cells[i, "D"] = GetStudentMarkForSpecialty(examList,
                        item.SessionEducationalSubjects.SessionId.Value,
                        item.SessionEducationalSubjects.Sessions.GroupId.Value).Min();

                    workSheet.Cells[i, "E"] = GetStudentMarkForSpecialty(examList,
                            item.SessionEducationalSubjects.SessionId.Value,
                            item.SessionEducationalSubjects.Sessions.GroupId.Value).Max();
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
        /// Method generation bad student by group.
        /// </summary>
        /// <param name="pathToFile">Path to file.</param>
        /// <param name="sortableSheet">Sorted table number.</param>
        /// <param name="sortOrder">Sort order.</param>
        public void GenerationBadStudentByGroup(string pathToFile, int sortableSheet, SortOrder sortOrder)
        {
            Application application = new Application();
            Workbook workBook = application.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Groups";
            workSheet.Cells[1, "B"] = "Students";

            int i = 2;
            int tempMark = 4;
            int idCount = 0;
            string creditResultIsNotPassed = "Not Passed";
            List<int> temp = StudentsDataContext.StudentResults.Select(item => item.StudentId.Value).Distinct().ToList();

            foreach (StudentResults item in StudentsDataContext.StudentResults)
            {
                int.TryParse(item.Mark, out tempMark);
                idCount = temp.Where(obj => obj == item.StudentId).Count();
                if (idCount == 1 && ((tempMark < 4 && tempMark != 0) || item.Mark == creditResultIsNotPassed))
                {
                    workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Sessions.Groups.Name;
                    workSheet.Cells[i, "B"] = item.Students.FullName;
                    temp.Add(item.StudentId.Value);
                    i++;
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
        /// Method get student mark for group.
        /// </summary>
        /// <param name="listStudentResults">List student results.</param>
        /// <param name="sessionId">Sessions id.</param>
        /// <param name="groupId">Groups id.</param>
        /// <returns>List SessionEducationalSubjects objects.</returns>
        private static List<double> GetStudentMarkForSpecialty(List<StudentResults> listStudentResults, int sessionId, int specialtyId)
        {
            return listStudentResults.Where(item => item.SessionEducationalSubjects.SessionId == sessionId
            && item.SessionEducationalSubjects.Sessions.Groups.SpecialtyId == specialtyId)
                .Select(item => double.Parse(item.Mark)).ToList();
        }
    }
}
