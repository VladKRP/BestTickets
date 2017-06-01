using BestTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteHelpBot.Extensions
{
    public static class TextFeedbackGenerator
    {
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
            var feedback = new List<string>() { "Возможно билетов больше нет.",
                "Билетов по вашему запросу не найденно.Возможно вы указали что-то не верно.",
                "Мне не удалось ничего найти. Возможно что-то указано не верно.",
                "Возможно вы в чем-то ошиблись. Мне не удалось ничего найти.",
           "По вашему запросу ничего не найдено. Возможно вы что-то ввели неверно." };
            return ReturnRandomString(feedback);
        }

        public static string MakeWrongRouteFeedbackUntrivial()
        {
            var feedback = new List<string>() { "Похоже вы не указали место отправления или прибытия.",
                "Маршрут не указан.",
                "Не могу определить маршрут.",
                "Я не смог определить маршрут. Попробуйте ввести еще раз." };
            return ReturnRandomString(feedback);
        }

        public static string MakeGreetingFeedbackUntrivial()
        {
            var feedback = new List<string>() { "Привет", "Я в вашем распоряжении", "Привет. Чем я могу вам помочь?" };
            return ReturnRandomString(feedback);

        }

        private static string ReturnRandomString(IEnumerable<string> elems)
        {
            return elems.ElementAt(new Random().Next(elems.Count()));
        }

    }
}