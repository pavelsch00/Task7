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
            report.GenerateAverageSessionReport(4, XlSortOrder.xlAscending);
        }
    }
}
