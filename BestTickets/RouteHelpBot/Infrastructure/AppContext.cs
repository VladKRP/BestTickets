using System.Data.Entity;

namespace RouteHelpBot.Infrastructure
{
    public class AppContext:DbContext
    {
        public AppContext(): base("CityHandbook"){ }

        public DbSet<City> Cities { get; set; }
    }
}