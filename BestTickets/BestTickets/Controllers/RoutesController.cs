using BestTickets.Infrastructure;
using BestTickets.Models;
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

        public IQueryable<RouteViewModel> GetTop10Routes()
        {
            var top10Requests = _context.GetTop10().Select(x => x.Route);
            return top10Requests;
        }
    }
}
