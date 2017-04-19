using System.Collections.Generic;
using System.Linq;

namespace BestTickets.Models
{
    public static class TicketCollectionExtension
    {
        public static double GetAverageTicketsPrice(this IEnumerable<Vehicle> tickets)
        {
            var ticketPrices = tickets.SelectMany(x => x.Places.Select(y => y.Cost));
            double averageTicketPrice = 0;
            if(ticketPrices.Count() != 0)
                averageTicketPrice = ticketPrices.Average();
            return averageTicketPrice;
        }

        public static IEnumerable<Vehicle> OrderTicketsPriceByDesc(this IEnumerable<Vehicle> tickets)
        {
            return tickets.OrderBy(x => x.Places.Min());
        }
    }
}
