namespace RouteHelpBot.Migrations
{
    using DAL;
    using System.Data.Entity.Migrations;
    using System.Web.Hosting;

    internal sealed class Configuration : DbMigrationsConfiguration<RouteHelpBot.DAL.CitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CitiesContext context)
        {
            //var fileName = HostingEnvironment.MapPath("~/Content/cities.xls");
            var fileName = @"D:\MyRepo\BestTickets\RouteHelpBot\RouteHelpBot\Content\cities.xls";
            var tableData = CityHandbookGenerator.ExtractExcelFile(fileName, "CITY");
            var cities = CityHandbookGenerator.ExtractCities(tableData);
            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
