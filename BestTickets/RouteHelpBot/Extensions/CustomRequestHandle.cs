using AdaptiveCards;
using BestTickets.Extensions;
using BestTickets.Models;
using BestTickets.Services;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteHelpBot.Extensions
{
    public static class  CustomRequestHandle
    {

        public static AdaptiveCard CreateFillRouteAdaptiveCard()
        {
            AdaptiveCard card = new AdaptiveCard();
            var inputFields = new List<TextInput>() {
                new TextInput()
                {
                    Id = "departurePlace",
                    IsRequired = true,
                    Placeholder = "Место отправления",
                },
                new TextInput()
                {
                    Id = "arrivalPlace",
                    IsRequired = true,
                    Placeholder = "Место прибытия",
                }
            };
            card.Body.Add(new TextBlock() { Text = "Введите маршрут:" });
            card.Body.AddRange(inputFields);
            card.Actions.Add(new SubmitAction()
            {
                Title = "Найти",

            });
            return card;
        }

        public static RouteViewModel CreateRouteByTextRequest(Activity activity)
        {
            var request = activity.Text;
            var departurePlace = request.Where((x, i) => i > request.IndexOf("из", StringComparison.CurrentCultureIgnoreCase) + 2 && i < request.IndexOf("в", StringComparison.CurrentCultureIgnoreCase) - 2)
                .TakeWhile(x => char.IsLetter(x)).Aggregate("", (x,y) => x+=y);
            var arrivalPlace = request.Where((x, i) => i > request.IndexOf("в", StringComparison.CurrentCultureIgnoreCase) + 1).TakeWhile(x => char.IsLetter(x)).Aggregate("", (x,y) => x+=y);           
            RouteViewModel route = new RouteViewModel(departurePlace, arrivalPlace, null);
            route.Date = route.SetCurrentDate();
            return route;      
        }

        public static string DefineVehicleType(Activity activity)
        {
            var request = activity.Text;
            var vehicleKind = request.Where((x, i) => i > request.IndexOf("на", StringComparison.CurrentCultureIgnoreCase) + 2).TakeWhile(x => char.IsLetter(x)).Aggregate("", (x, y) => x += y);
            return vehicleKind;
        }

        public static string HandleTextRequest(RouteViewModel route, string vehicleKind = null)
        {
            string responseText;
            if (string.IsNullOrEmpty(route.ArrivalPlace) || string.IsNullOrEmpty(route.DeparturePlace))
                responseText = MakeTicketsNotFoundFeedbackUntrivial();
            else
            {
                var tickets = FilterByVehicleKind(route, vehicleKind);
                responseText = GenerateTicketsFeedbackMessage(tickets);
            }
            return responseText;
        }

        private static IEnumerable<Vehicle> FilterByVehicleKind(RouteViewModel route, string vehicleKind)
        {
            IEnumerable<Vehicle> tickets = null;
                
            if (vehicleKind.IndexOf("маршрутк", StringComparison.CurrentCultureIgnoreCase) > 0 || vehicleKind.IndexOf("автобус", StringComparison.CurrentCultureIgnoreCase) > 0)
                tickets = TicketChecker.FindBusTickets(route);
            else if (vehicleKind.IndexOf("поезд", StringComparison.CurrentCultureIgnoreCase) > 0 || vehicleKind.IndexOf("электричк", StringComparison.CurrentCultureIgnoreCase) > 0)
                tickets = TicketChecker.FindTrainTickets(route);
            else
                tickets = TicketChecker.FindTickets(route);
            return tickets;
        }

        public static string GenerateTicketsFeedbackMessage(IEnumerable<Vehicle> tickets)
        {
            var feedbackMessage = new StringBuilder();
            if (tickets.Count() > 0)
            {
                feedbackMessage.Append($"Вот что я нашел по вашему запросу:\n ");
                foreach (var ticket in tickets)
                {

                    feedbackMessage.Append($"---\n\r {ticket.Name} {ticket.Route} \n\r {ticket.Type}\n\r Отправление:{ticket.DepartureTime}\n\r Прибытие:{ticket.ArrivalTime}\n\r ");
                    if (ticket.Places.Count() > 0)
                    {
                        feedbackMessage.Append($"Места:\n\r ");
                        foreach (var place in ticket.Places)
                            feedbackMessage.Append($"{place.Type}/ {place.Amount}/ {place.Cost} руб.\n\r ");
                    }

                }
            }
            else
                feedbackMessage.Append(MakeTicketsNotFoundFeedbackUntrivial());
            return feedbackMessage.ToString();
        }

        public static string MakeTicketsNotFoundFeedbackUntrivial()
        {
            var feedback = new List<string>() { "Билетов по вашему запросу не найденно.Возможно вы указали что-то не верно.",
                "Мне не удалось ничего найти. Возможно что-то указано не верно.",
                "Возможно вы в чем-то ошиблись. Мне не удалось ничего найти.",
           "По вашему запросу ничего не найдено. Возможно вы что-то ввели неверно." };
            return feedback.ElementAt(new Random().Next(feedback.Count()));
        }
    }
}