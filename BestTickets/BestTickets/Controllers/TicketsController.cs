using BestTickets.Attributes;
using BestTickets.Extensions;
using BestTickets.Infrastructure;
using BestTickets.Models;
using BestTickets.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace BestTickets.Controllers
{
    public class TicketsController : ApiController
    {
        private IRouteRequestRepository context;

        public TicketsController()
        {
            context = new RouteRequestRepository();
        }

        public TicketsController(IRouteRequestRepository repo)
        {
            context = repo;
        }

        [WebApiCache(Duration = 30)]
        public IEnumerable<Vehicle> GetTickets([FromUri]RouteViewModel route)
        {
            if (route.Date == null)
                route.Date = route.SetCurrentDate();

            var tickets = TicketChecker.FindTickets(route);
            var averagePrice = tickets.GetAverageTicketsPrice();
            var groupedTickets = tickets.GroupTicketsByAveragePrice(averagePrice);
            if(tickets.Count() != 0)
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
