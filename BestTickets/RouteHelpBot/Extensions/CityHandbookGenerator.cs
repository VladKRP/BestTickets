using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Infrastructure.City> ExtractCities(Range range)
        {
            int rowCount = range.Rows.Count;
            var cities = Enumerable.Range(1, rowCount).Where(x => range.Cells[x,2] != null && range.Cells[x,2].Value2 != null)
                .Select(x => new Infrastructure.City { Id = x, Name = range.Cells[x, 2].Value2.ToString() } );
            return cities;
        }

    }
}