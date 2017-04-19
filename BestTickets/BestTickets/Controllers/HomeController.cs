using System.Web.Mvc;
using BestTickets.Models;

namespace BestTickets.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
             return View();
        }

        public ActionResult GetTickets(RouteViewModel route)
        {
            if (route.Date == null)
                route.Date = route.SetCurrentDate();

            var tickets = TicketChecker.FindTickets(route).OrderTicketsPriceByDesc();
            return PartialView("_GetTickets", tickets);
        }
    }
}