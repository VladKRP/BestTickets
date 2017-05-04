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
                var places = ticket.Places.ToList();
                foreach (var place in places)
                {
                    if (place.Cost <= averagePrice)
                        place.isCostLessThanAverage = true;
                }
                ticket.Places = places;
                yield return ticket;
            }
        }
    }
}
