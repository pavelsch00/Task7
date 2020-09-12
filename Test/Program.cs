using Epam_Task7.Enums;
using Epam_Task7.Reports;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerationReport report = new GenerationReport();
            string path = @"..\..\..\..\..\Task7\Epam_Task7\Resources\Report3.xlsx";
            report.GenerationAverageResultStudentByYear(path, 4, SortOrder.Ascending);
        }
    }
}
