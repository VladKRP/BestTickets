using System.Collections.Generic;
using System.Linq;
using BestTickets.Models;

namespace BestTickets.Extensions
{
    public static class TicketCollectionExtension
    {
        public static double GetAverageTicketsPrice(this IEnumerable<Vehicle> tickets)
        {
            var ticketPrices = tickets.SelectMany(x => x.Places.Select(y => y.Cost));
            double averageTicketPrice = 0;
            if(ticketPrices.Count() != 0)
                averageTicketPrice = (double)ticketPrices.Average();
            return averageTicketPrice;
        }

        public static IEnumerable<Vehicle> OrderTicketsPriceByDesc(this IEnumerable<Vehicle> tickets)
        {
            return tickets.OrderBy(x => x.Places.Min());
        }

        public static IEnumerable<Vehicle> GroupTicketsByAveragePrice(this IEnumerable<Vehicle> tickets, double averagePrice)
        {
            foreach (var ticket in tickets)
            {
                foreach (var place in ticket.Places)
                {
                    if (place.Cost <= averagePrice)
                        place.isCostLessThanAverage = true;
                }
                yield return ticket;
            }
        }
    }
}
