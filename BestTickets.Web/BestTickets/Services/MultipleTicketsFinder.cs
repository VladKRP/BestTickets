using BestTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTickets.Services
{
    public class MultipleTicketsFinder:ITicketsFinder
    {
        public IEnumerable<Vehicle> SearchTickets(Route route)
        {
            return new TicketBusTicketsFinder().SearchTickets(route).Concat(new RaspRwTicketsFinder().SearchTickets(route));
        }

    }
}