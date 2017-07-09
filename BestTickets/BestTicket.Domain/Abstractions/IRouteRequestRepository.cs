using System.Linq;
using BestTickets.Domain.Models;

namespace BestTickets.Domain.Abstractions
{
    public interface IRouteRequestRepository:IRepository<RouteRequest>
    {
        IQueryable<RouteRequest> GetTop10();
        RouteRequest GetByRoute(Route route);
    }
}