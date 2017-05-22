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

        public static IEnumerable<Vehicle> OrderTicketsByPrice(this IEnumerable<Vehicle> tickets, bool? descending)
        {
            IEnumerable<Vehicle> orderedTickets;
            if(descending == true)
                orderedTickets = tickets.OrderBy(x => x.Places.Min());
            else
                orderedTickets = tickets.OrderByDescending(x => x.Places.Min());
            return orderedTickets;
        }

        public static IEnumerable<Vehicle> OrderTicketsByDepartureTime(this IEnumerable<Vehicle> tickets, bool? descending)
        {
            IEnumerable<Vehicle> orderedTickets;
            if (descending == true)
                orderedTickets = tickets.OrderByDescending(x => x.DepartureTime);
            else
                orderedTickets = tickets.OrderBy(x => x.DepartureTime);
            return orderedTickets;
        }

        public static IEnumerable<Vehicle> FilterTicketsByPrice(this IEnumerable<Vehicle> tickets, int startPrice, int endPrice)
        {
            return tickets.Where(x => x.Places.Min().Cost >= startPrice && x.Places.Max().Cost <= endPrice);
        }

        public static IEnumerable<Vehicle> SortByParametr(this IEnumerable<Vehicle> tickets, string param, bool? isDescending)
        {
            switch (param)
            {
                case "time": tickets = tickets.OrderTicketsByDepartureTime(isDescending); break;
                case "price": tickets = tickets.OrderTicketsByPrice(isDescending);break;
            }
            return tickets;
        }

       
    }
}
