using System.Data.Entity.Migrations;

namespace BestTickets.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<BestTickets.Infrastructure.RouteRequestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BestTickets.Infrastructure.RouteRequestContext context)
        {
        }
    }
}
