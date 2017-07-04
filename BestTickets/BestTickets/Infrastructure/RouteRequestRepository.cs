using BestTickets.Models;
using System.Linq;
using System.Collections.Generic;

namespace BestTickets.Infrastructure
{
    public class RouteRequestRepository:IRouteRequestRepository
    {
        private RouteRequestContext context;

        public RouteRequestRepository()
        {
            context = new RouteRequestContext();
        }

        public RouteRequestRepository(RouteRequestContext context)
        {
            this.context = context;
        }

        public IEnumerable<RouteRequest> GetAll() => context.RouteRequests.Select(x => x);

        public IQueryable<RouteRequest> GetTop10() => context.RouteRequests.OrderByDescending(x => x.RequestsCount).Take(10);
        
        public RouteRequest FindById(int id) => context.RouteRequests.Find(id);

        public RouteRequest FindByRoute(RouteViewModel route)
        {
            var routeRequest = context.RouteRequests
                .FirstOrDefault(x => x.Route.ArrivalPlace.Equals(route.ArrivalPlace) && x.Route.DeparturePlace.Equals(route.DeparturePlace));
            return routeRequest;
        }

        public void Create(RouteRequest routeRequest) => context.RouteRequests.Add(routeRequest);

        public void Update(RouteRequest routeRequest) => context.Entry(routeRequest).State = System.Data.Entity.EntityState.Modified;

        public void Delete(int id)
        {
            RouteRequest routeRequest = context.RouteRequests.Find(id);
            context.RouteRequests?.Remove(routeRequest);
        }

        public void Save() => context.SaveChanges();

    }
}