using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BestTickets.Infrastructure;
using BestTickets.Models;
using BestTickets.Services;
using BestTickets.Extensions;

namespace BestTickets.Controllers
{
    public class TicketsController : ApiController
    {

        private IRepository<RouteRequest> context;

        public TicketsController()
        {
            context = new RouteRequestRepository();
        }

        public TicketsController(IRepository<RouteRequest> repo)
        {
            context = repo;
        }

        public IEnumerable<Vehicle> GetTickets(RouteViewModel route)
        {

            if (route.Date == null)
                route.Date = route.SetCurrentDate();

            var tickets = TicketChecker.FindTickets(route).OrderTicketsPriceByDesc();
            var averagePrice = tickets.GetAverageTicketsPrice();
            var groupedTickets = tickets.GroupTicketsByAveragePrice(averagePrice);

            if (groupedTickets != null)
                UpdateOrCreateRouteIfNotExist(route);

            return groupedTickets;
        }

        private void UpdateOrCreateRouteIfNotExist(RouteViewModel route)
        {
            var routeRequest = context.FindByRoute(route);
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
