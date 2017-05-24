using BestTickets.Models;
using System.Linq;
using System.Collections.Generic;

namespace BestTickets.Infrastructure
{
    public class RouteRequestRepository:IRepository<RouteRequest>
    {
        private RouteRequestContext context;

        public RouteRequestRepository()
        {
            context = new RouteRequestContext();
        }

        public IEnumerable<RouteRequest> GetAll()
        {
            return context.RouteRequests.Select(x => x);
        }

        public IQueryable<RouteRequest> GetTop10()
        {
            return context.RouteRequests.OrderByDescending(x => x.RequestsCount).Take(10);
        }
        
        public RouteRequest FindById(int id)
        {
            return context.RouteRequests.Find(id);
        }

        public RouteRequest FindByRoute(RouteViewModel route)
        {
            var routeRequest = context.RouteRequests
                .FirstOrDefault(x => x.Route.ArrivalPlace.Equals(route.ArrivalPlace) && x.Route.DeparturePlace.Equals(route.DeparturePlace));
            return routeRequest;
        }

        public void Create(RouteRequest routeRequest)
        {
            context.RouteRequests.Add(routeRequest);
        }

        public void Update(RouteRequest routeRequest)
        {
            context.Entry(routeRequest).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(int id)
        {
            RouteRequest routeRequest = context.RouteRequests.Find(id);
            context.RouteRequests?.Remove(routeRequest);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}