using BestTickets.Domain.Models;
using BestTickets.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace BestTickets.Services
{
    public class TicketBusTicketsFinder: ITicketsFinder, IHtmlTicketsFinder
    {
        private readonly string siteUrl = "http://ticketbus.by/";
        private string phpSessionId = "";

        public IEnumerable<Vehicle> SearchTickets(Route route)
        {
            IEnumerable<Vehicle> tickets = Enumerable.Empty<Vehicle>();
            var ticketBusContent = HtmlHandler.ReadHtmlPage(siteUrl);
            if (ticketBusContent != "Service don't work yet.")
            {
                phpSessionId = ticketBusContent.Skip(ticketBusContent.IndexOf("var url")).Skip(11).TakeWhile(x => x != '"').Aggregate("", (x, y) => x += y);
                tickets = FindTicketsInHtmlMarkup(LoadTicketsMarkup(route));
            }
            return tickets;
        }

        public IEnumerable<Vehicle> FindTicketsInHtmlMarkup(string html)
        {
            var htmlDocument = HtmlHandler.LoadHtmlRootElement(html);
            var tickets = from ticket in HtmlHandler.GetElementByClass(htmlDocument, "odd").Concat(HtmlHandler.GetElementByClass(htmlDocument, "even"))
                          select new Vehicle()
                          {
                              Name = HtmlHandler.GetElementValueByClass(ticket, ""),
                              Type = HtmlHandler.GetElementValueByClass(ticket, "typ"),
                              Kind = "Маршрутка/Автобус",
                              Route = HtmlHandler.GetElementValueByClass(ticket, "marshrut"),
                              DepartureTime = HtmlHandler.GetFirstElementValueByClass(ticket, "time"),
                              ArrivalTime = HtmlHandler.GetLastElementValueByClass(ticket, "time"),
                              Places = new List<VehiclePlace>()
                              {
                                  new VehiclePlace("Сидя",
                                                    HtmlHandler.GetElementValueByClass(ticket, "bus-info"),
                                                    HtmlHandler.GetElementValueByClass(ticket, "price"))
                              }

                          };
            return tickets;
        }

        private string FindCityId(string cities, string city)
        {
            var htmlDocument = HtmlHandler.LoadHtmlRootElement(cities);
            return htmlDocument.Descendants().Where(x => x.InnerText.ToLower().Contains(city.ToLower()))
                               .Select(x => x.PreviousSibling.Attributes["value"].Value).FirstOrDefault();
        }

        private string GetCities()
        {
            var cityRequestUrl = string.Concat(siteUrl, phpSessionId);
            var siteCities = CustomRequest.SendRequest(cityRequestUrl, "POST", "prog=getcity", siteUrl);
            return siteCities;
        }

        private string GenerateTicketsRequestData(Route route)
        {
            var date = (System.DateTime)route.Date;
            var cities = GetCities();    
            var departureCityId = FindCityId(cities, route.DeparturePlace);
            var arrivalCityId = FindCityId(cities, route.ArrivalPlace);
            var ticketsRequest = $"station_id={arrivalCityId}&station_id1={departureCityId}&date={date.ToString("dd.MM.yyyy")}";
            return ticketsRequest;
        }

        private string LoadTicketsMarkup(Route route)
        {
            var ticketsRequestUrl = string.Concat(siteUrl, phpSessionId, "&prog=marshrut1&host=1");
            var ticketsRequestData = GenerateTicketsRequestData(route);
            return CustomRequest.SendRequest(ticketsRequestUrl, "POST", ticketsRequestData, siteUrl);
        }
    }
}