using BestTickets.Domain.Models;
using BestTickets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BestTickets.Services
{
    public class RaspRwTicketsFinder:ITicketsFinder, IHtmlTicketsFinder
    {
        private readonly string siteUrl = "http://rasp.rw.by/ru/route/";

        public IEnumerable<Vehicle> SearchTickets(Route route)
        {
            IEnumerable<Vehicle> tickets = Enumerable.Empty<Vehicle>();
            var date = (DateTime)route.Date;
            string raspRwUrl = string.Format($"{siteUrl}?from={route.DeparturePlace}&to={route.ArrivalPlace}&date={date.ToString("yyyy-MM-dd")}");
            var raspRwContent = HtmlHandler.ReadHtmlPage(raspRwUrl).Result;
            if (raspRwContent != "Service don't work yet.")
                tickets = FindTicketsInHtmlMarkup(raspRwContent);
            return tickets;
        }

        public IEnumerable<Vehicle> FindTicketsInHtmlMarkup(string html)
        {
            var htmlDocument = HtmlHandler.LoadHtmlRootElement(html);
            var ticketsInfoNodes = HtmlHandler.GetElementByClass(htmlDocument, "schedule_list")
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "tr"));
            var tickets = from ticket in ticketsInfoNodes
                          select new Vehicle()
                          {
                              Name = HtmlHandler.GetElementValueByClass(ticket, "train_id"),
                              Type = HtmlHandler.GetElementValueByClass(ticket, "train_description"),
                              Kind = "Поезд/Электричка",
                              Route = HtmlHandler.GetElementValueByTag(HtmlHandler.GetElementByClass(ticket, "train_name -map").FirstOrDefault(), "a")
                                                 .FirstOrDefault()
                                                 .Replace("&nbsp;", "")
                                                 .Replace("&mdash;", " - "),
                              DepartureTime = HtmlHandler.GetElementValueByClass(ticket, "train_start-time"),
                              ArrivalTime = HtmlHandler.GetElementValueByClass(ticket, "train_end-time"),
                              Places = from place in HtmlHandler.GetElementByClass(ticket, "train_details-group")
                                       select new VehiclePlace(
                                         HtmlHandler.GetElementValueByClass(place, "train_note"),
                                         HtmlHandler.GetElementValueByClass(place, "train_place"),
                                         HtmlHandler.GetFirstElementValueByClass(place, "denom_after")
                                            )
                          };
            return tickets;
        }

    }
}