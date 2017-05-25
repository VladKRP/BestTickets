using BestTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTickets.Infrastructure
{
    public class FakeRouteRequestRepository:IRepository<RouteRequest>
    {
        private IEnumerable<RouteRequest> context;

        public FakeRouteRequestRepository(IEnumerable<RouteRequest> requests)
        {
            context = requests;
        }

        public IEnumerable<RouteRequest> GetAll()
        {
            return context.Select(x => x);
        }

        public IQueryable<RouteRequest> GetTop10()
        {
            var top10Requests =  context.OrderByDescending(x => x.RequestsCount).Take(10);
            return top10Requests.AsQueryable();
        }

        public RouteRequest FindById(int id)
        {
            return context.FirstOrDefault(x => x.Id.Equals(id));
        }

        public RouteRequest FindByRoute(RouteViewModel route)
        {
            var routeRequest = context
                .FirstOrDefault(x => x.Route.ArrivalPlace.Equals(route.ArrivalPlace) && x.Route.DeparturePlace.Equals(route.DeparturePlace));
            return routeRequest;
        }

        public void Create(RouteRequest routeRequest) {}
        public void Update(RouteRequest routeRequest) {}
        public void Delete(int id){}
        public void Save(){}
    }
}