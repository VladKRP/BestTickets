using BestTickets.Domain.Models;
using System.Collections.Generic;

namespace BestTickets.Services
{
    public interface ITicketsFinder
    {
        IEnumerable<Vehicle> SearchTickets(Route route);
    }
}
