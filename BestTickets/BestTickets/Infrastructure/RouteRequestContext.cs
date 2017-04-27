using System.Data.Entity;
using BestTickets.Models;

namespace BestTickets.Database
{
    public class RouteRequestContext:DbContext
    {
        DbSet<RouteRequest> RouteRequests { get; set; }
    }
}