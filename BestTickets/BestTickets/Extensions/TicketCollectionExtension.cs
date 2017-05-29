using System.Collections.Generic;
using System.Linq;
using BestTickets.Models;
using System;

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

        public static IEnumerable<Vehicle> GetTicketsByTime(this IEnumerable<Vehicle> tickets, string time)
        {
            return tickets.Where(x => x.DepartureTime.Equals(time));
        }

        public static Vehicle GetImmediateTicket(this IEnumerable<Vehicle> tickets)
        {
            //idk work right, or no
            TimeSpan time = new TimeSpan();
            return tickets.OrderBy(x => x.DepartureTime).FirstOrDefault(x => new TimeSpan(int.Parse(x.DepartureTime.Take(2).ToString()),int.Parse(x.DepartureTime.Skip(3).Take(2).ToString()),0) <= time);
        }

        //public static IEnumerable<Vehicle> GetImmediateTicketsByTime(this IEnumerable<Vehicle> tickets, string time) { return null; }

        public static IEnumerable<Vehicle> GetTicketsByPrice(this IEnumerable<Vehicle> tickets, int price)
        {
            return tickets.Where(x => x.Places.Min().Cost <= price);
        }

    }
}
