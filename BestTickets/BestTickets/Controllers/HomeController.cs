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
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}