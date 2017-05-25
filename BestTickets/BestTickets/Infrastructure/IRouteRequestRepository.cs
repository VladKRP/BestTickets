using BestTickets.Models;
using System.Collections.Generic;
using System.Linq;

namespace BestTickets.Infrastructure
{
    public interface IRouteRequestRepository
    {
        IEnumerable<RouteRequest> GetAll();
        IQueryable<RouteRequest> GetTop10();
        RouteRequest FindById(int id);
        RouteRequest FindByRoute(RouteViewModel route);
        void Create(RouteRequest route);
        void Update(RouteRequest route);
        void Delete(int id);
        void Save();
    }
}
