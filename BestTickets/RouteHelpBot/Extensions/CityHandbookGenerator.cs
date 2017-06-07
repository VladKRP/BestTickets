using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;

namespace RouteHelpBot.Extensions
{
    public static class CityHandbookGenerator
    {
        public static Range ExtractExcelFile(string fileName, string tableName)
        {
            Application xlApp = new Application();
            Workbook xlWorkbook = xlApp.Workbooks.Open(fileName);
            _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            return xlWorksheet.UsedRange;
        }

        public static IEnumerable<Model.City> ExtractCities(Range range)
        {
            int rowCount = range.Rows.Count;
            for(int i = 2; i< rowCount; i++)
            {
                if (range.Cells[i, 2] != null && range.Cells[i, 2].Value2 != null)
                    yield return new Model.City() { Id = i - 1, Name = range.Cells[i, 2].Value2.ToString() };
            }
        }

    }
}