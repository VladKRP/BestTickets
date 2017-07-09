using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BestTickets.Domain.Models;

namespace BestTickets.Infrastructure
{
    public class RouteRequestContext:DbContext
    {
        public RouteRequestContext() : base("BestTicketsDBConnection"){}

        public DbSet<RouteRequest> RouteRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}