using System.Linq;
using System.Collections.Generic;
using BestTickets.Domain.Models;
using BestTickets.Domain.Abstractions;

namespace BestTickets.Infrastructure
{
    public class EFRouteRequestRepository:IRouteRequestRepository
    {
        private RouteRequestContext context;

        public EFRouteRequestRepository()
        {
            context = new RouteRequestContext();
        }

        public EFRouteRequestRepository(RouteRequestContext context)
        {
            this.context = context;
        }

        public IEnumerable<RouteRequest> GetAll() => context.RouteRequests.Select(x => x);

        public IQueryable<RouteRequest> GetTop10() => context.RouteRequests.OrderByDescending(x => x.RequestsCount).Take(10);
        
        public RouteRequest Get(int id) => context.RouteRequests.Find(id);

        public RouteRequest GetByRoute(Route route) => context.RouteRequests
                .FirstOrDefault(x => x.Route.ArrivalPlace.Equals(route.ArrivalPlace) && x.Route.DeparturePlace.Equals(route.DeparturePlace));


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