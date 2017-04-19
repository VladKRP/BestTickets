using System.Collections.Generic;
using System.Linq;

namespace BestTickets.Models
{
    public class TicketChecker
    {
        public static IEnumerable<Vehicle> FindTickets(RouteViewModel route)
        {
            string raspRwUrl = string.Format($"http://rasp.rw.by/ru/route/?from={route.DeparturePlace}&to={route.ArrivalPlace}&date={DateFormatChange(route.Date, "-", true)}");
            var raspRwContent = Parser.ParseSiteAsString(raspRwUrl);
            var raspRwTickets = RaspRwSearch(raspRwContent);

            string ticketBusUrl = "http://ticketbus.by/";
            var ticketBusContent = Parser.ParseSiteAsString(ticketBusUrl);
            var ticketBusPHPSessionId = ticketBusContent.Skip(ticketBusContent.IndexOf("var url")).Skip(11).TakeWhile(x => x != '"').Aggregate("", (x, y) => x += y);
            var ticketBusTickets = TicketBusSearch(TicketBusGetData(route, ticketBusUrl, ticketBusPHPSessionId));

            return raspRwTickets.Concat(ticketBusTickets);

        }

        private static string DateFormatChange(string departureDate, string separator, bool yearMonthDay = true)
        {
            if (departureDate == null)
                return null;
            var date = departureDate.Split('/', '.', '-').Select(x => x);
            if (!yearMonthDay)
                date = date.Reverse();
            return string.Join(separator, date);
        }

        private static IEnumerable<Vehicle> RaspRwSearch(string siteContent)
        {
            var htmlDocument = Parser.LoadHtmlRootElement(siteContent);
            var ticketsInfoNodes = Parser.GetElementByClass(htmlDocument, "schedule_list")
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "tr"));
            var tickets = from ticket in ticketsInfoNodes
                          select new Vehicle()
                          {
                              Name = Parser.GetElementValueByClass(ticket, "train_id"),
                              Type = Parser.GetElementValueByClass(ticket, "train_description"),
                              Route = Parser.GetElementValueByTag(Parser.GetElementByClass(ticket, "train_name -map").FirstOrDefault(), "a").FirstOrDefault()
                                              .Replace("&nbsp;", "").Replace("&mdash;", " - "),
                              DepartureTime = Parser.GetElementValueByClass(ticket, "train_start-time"),
                              ArrivalTime = Parser.GetElementValueByClass(ticket, "train_end-time"),
                              Places = from place in Parser.GetElementByClass(ticket, "train_details-group")
                                       select new VehiclePlace(
                                         Parser.GetElementValueByClass(place, "train_note"),
                                         Parser.GetElementValueByClass(place, "train_place"),
                                         Parser.GetFirstElementValueByClass(place, "denom_after")
                                            )
                          };
            return tickets;
        }

        private static string TicketBusFindCityId(string siteCities, string city)
        {
            var htmlDocument = Parser.LoadHtmlRootElement(siteCities);
            return htmlDocument.Descendants().Where(x => x.InnerText.ToLower().Contains(city.ToLower())).Select(x => x.PreviousSibling.Attributes["value"].Value).FirstOrDefault();
        }


        private static string TicketBusGetData(RouteViewModel route, string url, string phpSessionId)
        {
            var cityRequestUrl = string.Concat(url, phpSessionId);
            var ticketsRequestUrl = string.Concat(cityRequestUrl, "&prog=marshrut1&host=1");
            var siteCities = Parser.SendPostRequest("prog=getcity", cityRequestUrl, url);
            var departureCityId = TicketBusFindCityId(siteCities, route.DeparturePlace);
            var arrivalCityId = TicketBusFindCityId(siteCities, route.ArrivalPlace);
            var postData = string.Format($"station_id={arrivalCityId}&station_id1={departureCityId}&date={DateFormatChange(route.Date, ".", false)}");
            return Parser.SendPostRequest(postData, ticketsRequestUrl, url);
        }

        private static IEnumerable<Vehicle> TicketBusSearch(string siteContent)
        {
            var htmlDocument = Parser.LoadHtmlRootElement(siteContent);
            var tickets = from ticket in Parser.GetElementByClass(htmlDocument, "odd").Concat(Parser.GetElementByClass(htmlDocument, "even"))
                          select new Vehicle()
                          {
                              Name = Parser.GetElementValueByClass(ticket, ""),
                              Type = Parser.GetElementValueByClass(ticket, "typ"),
                              Route = Parser.GetElementValueByClass(ticket, "marshrut"),
                              DepartureTime = Parser.GetFirstElementValueByClass(ticket, "time"),
                              ArrivalTime = Parser.GetLastElementValueByClass(ticket, "time"),
                              Places = new List<VehiclePlace>()
                              {
                                  new VehiclePlace("Сидя",
                                                    Parser.GetElementValueByClass(ticket, "bus-info"),
                                                    Parser.GetElementValueByClass(ticket, "price"))
                              }
                                       
                          };
            return tickets;
        }

    }
}
