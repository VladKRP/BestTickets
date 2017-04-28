using System.Data.Entity;
using BestTickets.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BestTickets.Infrastructure
{
    public class RouteRequestContext:DbContext
    {
        public RouteRequestContext() : base("BestTicketsConnection")
        {
            Database.SetInitializer<RouteRequestContext>(new CreateDatabaseIfNotExists<RouteRequestContext>());
        }

        public DbSet<RouteRequest> RouteRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}