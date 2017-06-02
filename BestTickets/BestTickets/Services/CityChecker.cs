using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestTickets.Extensions;

namespace BestTickets.Services
{
    public static class CityChecker
    {
        public static IEnumerable<string> GetCities()
        {
            string wikiUrl = "https://ru.wikipedia.org/wiki/%D0%93%D0%BE%D1%80%D0%BE%D0%B4%D0%B0_%D0%91%D0%B5%D0%BB%D0%BE%D1%80%D1%83%D1%81%D1%81%D0%B8%D0%B8";
            var siteContent = Parser.ParseSiteAsString(wikiUrl);
            var htmlDocument = Parser.LoadHtmlRootElement(siteContent);
            var regionTables = Parser.GetElementById(htmlDocument, "mw-content-text").ChildNodes.Where(y => y.Name == "table");
            var bodyOfReqionTables = regionTables.SelectMany(x => x.Descendants().Where(y => y.Name == "tr").Skip(1));
            var citiesAndRegions = bodyOfReqionTables.SelectMany(x => x.Descendants().Where(z => z.Attributes["title"] != null).Select(z => z.InnerText));
            var cities = citiesAndRegions.Where((x, i) => i % 2 == 0);
            return null;
        }
    }
}
