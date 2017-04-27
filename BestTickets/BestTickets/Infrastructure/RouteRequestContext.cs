using System.Data.Entity;
using BestTickets.Models;

namespace BestTickets.Infrastructure
{
    public class RouteRequestContext:DbContext
    {
       public RouteRequestContext() : base("name=BestTicketsDB") { }
       public DbSet<RouteRequest> RouteRequests { get; set; }
    }
}