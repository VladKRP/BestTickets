using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BestTickets.Infrastructure;
using BestTickets.Models;

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

        [HttpGet]
        public IQueryable<RouteRequest> GetTop10Routes()
        {
            var top10Requests = context.GetTop10();
            return top10Requests;
        } 
    }
}
