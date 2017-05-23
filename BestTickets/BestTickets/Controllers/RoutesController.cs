using BestTickets.Infrastructure;
using BestTickets.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BestTickets.Controllers
{
    public class RoutesController : ApiController
    {
        private IRepository<RouteRequest> context;

        public RoutesController()
        {
            context = new RouteRequestRepository();
        }

        public RoutesController(IRepository<RouteRequest> repo)
        {
            context = repo;
        }

        public IQueryable<RouteViewModel> GetTop10Routes()
        {
            var top10Requests = context.GetTop10().Select(x => x.Route);
            return top10Requests;
        }
    }
}
