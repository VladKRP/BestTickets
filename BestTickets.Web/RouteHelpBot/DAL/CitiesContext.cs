using System.Data.Entity;

namespace RouteHelpBot.DAL
{
    public class CitiesContext:DbContext
    {
        public CitiesContext(): base("CitiesHandbook"){ }

        public virtual DbSet<Model.City> Cities { get; set; }

    }
}