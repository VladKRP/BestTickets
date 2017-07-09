using BestTickets.Domain.Models;
using System.Collections.Generic;

namespace BestTickets.Services
{
    public interface IHtmlTicketsFinder
    {
        IEnumerable<Vehicle> FindTicketsInHtmlMarkup(string html);
    }
}
