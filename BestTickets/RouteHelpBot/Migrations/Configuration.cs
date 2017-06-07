namespace RouteHelpBot.Migrations
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Hosting;

    internal sealed class Configuration : DbMigrationsConfiguration<RouteHelpBot.DAL.CitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RouteHelpBot.DAL.CitiesContext context)
        {
            //var fileName = HostingEnvironment.MapPath("~/Content/cities.xls");
            var fileName = @"D:\BestTickets\BestTickets\RouteHelpBot\Content\cities.xls";
            var tableData = CityHandbookGenerator.ExtractExcelFile(fileName, "CITY");
            var cities = CityHandbookGenerator.ExtractCities(tableData);
            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
