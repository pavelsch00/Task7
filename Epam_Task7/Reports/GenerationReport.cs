using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace Epam_Task7.Reports
{
    public class GenerationReport
    {
        public GenerationReport()
        {
            StudentsDataContext = new StudentsDataContext();
        }

        public StudentsDataContext StudentsDataContext { get; set; }

        public void GenerateSessionReport(int sortableSheet, XlSortOrder xlSortOrder)
        {
            Application excelApp = new Application();
            Workbook workBook = excelApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Session";
            workSheet.Cells[1, "B"] = "Group";
            workSheet.Cells[1, "C"] = "Student";
            workSheet.Cells[1, "D"] = "EducationSubject";
            workSheet.Cells[1, "E"] = "Type";
            workSheet.Cells[1, "F"] = "Mark";

            int i = 2;

            foreach (var item in StudentsDataContext.StudentResults)
            {
                workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Sessions.SessionNumber;
                workSheet.Cells[i, "B"] = item.SessionEducationalSubjects.Sessions.Groups.Name;
                workSheet.Cells[i, "C"] = item.Students.FullName;
                workSheet.Cells[i, "D"] = item.SessionEducationalSubjects.EducationalSubjects.SubjectName;
                workSheet.Cells[i, "E"] = item.SessionEducationalSubjects.EducationalSubjects.SubjectType;
                workSheet.Cells[i, "F"] = item.Mark;
                i++;
            }

            SortSheet(workSheet, i, sortableSheet, xlSortOrder);

            try
            {
                workBook.Close(true, $"{Environment.CurrentDirectory}" + @"..\..\..\..\..\Task7\Epam_Task7\Resources\Report1.xlsx");
                excelApp.Quit();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid path to file.");
            }
        }

        public void GenerateAverageSessionReport(int sortableSheet, XlSortOrder xlSortOrder)
        {
            Application excelApp = new Application();
            Workbook workBook = excelApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Session";
            workSheet.Cells[1, "B"] = "Group";
            workSheet.Cells[1, "C"] = "Average Mark";
            workSheet.Cells[1, "D"] = "Min Mark";
            workSheet.Cells[1, "E"] = "Max Mark";

            int i = 2;

            List<int> temp = StudentsDataContext.StudentResults.Select(item => item.SessionEducationalSubjects.SessionId.Value).Distinct().ToList();

            foreach (var item in StudentsDataContext.StudentResults)
            {
                if (temp.Where(obj => obj == item.SessionEducationalSubjects.SessionId).Select(obj => obj).Count() == 1)
                {
                    workSheet.Cells[i, "A"] = item.SessionEducationalSubjects?.Sessions.SessionNumber;
                    workSheet.Cells[i, "B"] = item.SessionEducationalSubjects?.Sessions.Groups.Name;

                    workSheet.Cells[i, "C"] = GetResultStudentForGroup(StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList(),
                        item.SessionEducationalSubjects.SessionId.Value,
                        item.SessionEducationalSubjects.Sessions.GroupId.Value).Average();

                    workSheet.Cells[i, "D"] = GetResultStudentForGroup(StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList(),
                        item.SessionEducationalSubjects.SessionId.Value,
                        item.SessionEducationalSubjects.Sessions.GroupId.Value).Min();

                    workSheet.Cells[i, "E"] = GetResultStudentForGroup(StudentsDataContext.StudentResults
                        .Where(obj => obj.SessionEducationalSubjects
                        .EducationalSubjects.SubjectType == "Exam")
                        .Select(obj => obj).ToList(),
                        item.SessionEducationalSubjects.SessionId.Value,
                        item.SessionEducationalSubjects.Sessions.GroupId.Value).Max();
                    i++;
                    temp.Add(item.SessionEducationalSubjects.SessionId.Value);
                }
            }

            SortSheet(workSheet, i, sortableSheet, xlSortOrder);

            try
            {
                workBook.Close(true, $"{Environment.CurrentDirectory}" + @"..\..\..\..\..\Task7\Epam_Task7\Resources\Report2.xlsx");
                excelApp.Quit();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid path to file.");
            }
        }

        public void GetBadStudent(int sortableSheet, XlSortOrder xlSortOrder)
        {
            Application excelApp = new Application();
            Workbook workBook = excelApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;

            workSheet.Cells[1, "A"] = "Group";
            workSheet.Cells[1, "B"] = "Student";

            int i = 2;
            int tempMark = 4;

            List<int> temp = StudentsDataContext.StudentResults.Select(item => item.StudentId.Value).Distinct().ToList();

            foreach (var item in StudentsDataContext.StudentResults)
            {
                int.TryParse(item.Mark, out tempMark);
                if (temp.Where(obj => obj == item.StudentId).Select(obj => obj).Count() == 1 &&
                    ((tempMark < 4 && tempMark != 0) || item.Mark == "Not Passed"))
                {
                    workSheet.Cells[i, "A"] = item.SessionEducationalSubjects.Sessions.Groups.Name;
                    workSheet.Cells[i, "B"] = item.Students.FullName;
                    temp.Add(item.StudentId.Value);
                    i++;
                }

            }

            SortSheet(workSheet, i, sortableSheet, xlSortOrder);

            try
            {
                workBook.Close(true, $"{Environment.CurrentDirectory}" + @"..\..\..\..\..\Task7\Epam_Task7\Resources\Report3.xlsx");
                excelApp.Quit();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid path to file.");
            }
        }

        private static void SortSheet(Worksheet workSheet, int maxLine, int sortableSheet, XlSortOrder xlSortOrder)
        {
            var rngSort = workSheet.get_Range("A1", $"F{maxLine}");
            rngSort.Sort(rngSort.Columns[sortableSheet, Type.Missing], xlSortOrder,
            null, Type.Missing, XlSortOrder.xlAscending,
            Type.Missing, XlSortOrder.xlAscending,
            XlYesNoGuess.xlYes, Type.Missing, Type.Missing,
            XlSortOrientation.xlSortColumns);
        }

        private static List<double> GetResultStudentForGroup(List<StudentResults> listStudentResults, int sessionId, int groupId)
        {
            return listStudentResults.Where(item => item.SessionEducationalSubjects.SessionId == sessionId
            && item.SessionEducationalSubjects.Sessions.GroupId == groupId)
                .Select(item => double.Parse(item.Mark)).ToList();
        }
    }
}
