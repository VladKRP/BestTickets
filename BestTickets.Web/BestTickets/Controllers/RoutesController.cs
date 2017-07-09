using BestTickets.Domain.Abstractions;
using BestTickets.Domain.Models;
using BestTickets.Infrastructure;
using System.Linq;
using System.Web.Http;

namespace BestTickets.Controllers
{
    public class RoutesController : ApiController
    {
        private readonly IRouteRequestRepository _context;

        public RoutesController()
        {
            _context = new RouteRequestRepository();
        }

        public RoutesController(IRouteRequestRepository repo)
        {
            _context = repo;
        }

        public IQueryable<Route> GetTop10Routes() => _context.GetTop10().Select(x => x.Route);
    }
}
