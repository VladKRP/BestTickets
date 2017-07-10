using BestTickets.Attributes;
using BestTickets.Extensions;
using BestTickets.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BestTickets.Domain.Models;
using BestTickets.Domain.Abstractions;
using BestTickets.Infrastructure;
using System;

namespace BestTickets.Controllers
{
    public class TicketsController : ApiController
    {
        private IRouteRequestRepository context;

        public TicketsController()
        {
            context = new EFRouteRequestRepository();
        }

        public TicketsController(IRouteRequestRepository repo)
        {
            context = repo;
        }

        [WebApiCache(Duration = 30)]
        public IEnumerable<Vehicle> GetTickets([FromUri]Route route)
        {
            if (route.Date.Value.Ticks == 0)
                route.Date = DateTime.Now;
            var tickets = new RaspRwTicketsFinder().SearchTickets(route).Concat(new TicketBusTicketsFinder().SearchTickets(route));
            var averagePrice = tickets.GetAverageTicketsPrice();
            var groupedTickets = tickets.GroupTicketsByAveragePrice(averagePrice);
            if (tickets.Count() != 0)
                UpdateOrCreateRouteIfNotExist(route);
            return groupedTickets;
        }

        private void UpdateOrCreateRouteIfNotExist(Route route)
        {
            var routeRequest = context.GetByRoute(route);
            if (routeRequest != null)
            {
                routeRequest.RequestsCount++;
                context.Update(routeRequest);
            }
            else
                context.Create(new RouteRequest() { Route = route });
            context.Save();
        }

    }
}
