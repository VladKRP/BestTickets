using System.Web.Mvc;
using BestTickets.Services;
using BestTickets.Models;
using BestTickets.Extensions;
using System.Linq;
using BestTickets.Infrastructure;

namespace BestTickets.Controllers
{
    public class HomeController : Controller
    {

        private IRouteRequestRepository context;

        public HomeController(IRouteRequestRepository repository)
        {
            context = repository;
        }

        public HomeController()
        {
            context = new RouteRequestRepository();
        }  

        public ActionResult Index()
        {
             return View("Index");
        }

        public ActionResult GetTickets(RouteViewModel route)
        {
            if (route.Date == null)
                route.Date = route.SetCurrentDate();

            var tickets = TicketChecker.FindTickets(route).OrderTicketsPriceByDesc();
            var averagePrice = tickets.GetAverageTicketsPrice();
            var groupedTickets = tickets.GroupTicketsByAveragePrice(averagePrice);
            string partialViewName;
            if (tickets.Count() == 0) partialViewName = "_TicketsNotFound";
            else
            {
                partialViewName = "_GetTickets";
                //UpdateRouteRequestsCount(route);
            }
                
            return PartialView(partialViewName, groupedTickets);
        }

        //
        public ActionResult GetTop10MostFrequentRequests()
        {
            var top10Requests = context.GetAll().OrderByDescending(x => x.RequestsCount).Take(10);
            return PartialView("_Top10FrequentRequests",top10Requests);
        }

        private void UpdateRouteRequestsCount(RouteViewModel route)
        {
            var routeRequest = context.FindByRoute(route);
            if (routeRequest != null)
            {
                routeRequest.RequestsCount++;
                context.Update(routeRequest);
                context.Save();
            }
            else
                context.Create(new RouteRequest() { Route = route });
        }

    }
}