using System.Web.Mvc;
using BestTickets.Services;
using BestTickets.Models;
using BestTickets.Extensions;
using System.Linq;
using BestTickets.Infrastructure;
using System.Collections.Generic;

namespace BestTickets.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<RouteRequest> context;

        public HomeController()
        {
            context = new RouteRequestRepository();
        }

        public HomeController(IRepository<RouteRequest> repo)
        {
            context = repo;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult GetTickets(RouteViewModel route)
        {
            string viewName;
            if (route.Date == null)
                route.Date = route.SetCurrentDate();

            var tickets = TicketChecker.FindTickets(route).OrderTicketsPriceByDesc();
            var averagePrice = tickets.GetAverageTicketsPrice();
            var groupedTickets = tickets.GroupTicketsByAveragePrice(averagePrice);

            if (groupedTickets != null)
            {
                UpdateOrCreateRouteIfNotExist(route);
                viewName = "_TicketsNotFound";
            }
            else
                viewName = "_GetTickets";
                

            return PartialView(viewName);
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