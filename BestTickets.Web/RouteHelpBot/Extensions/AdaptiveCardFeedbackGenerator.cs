using AdaptiveCards;
using BestTickets.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace RouteHelpBot.Extensions
{
    public class AdaptiveCardFeedbackGenerator
    {

        public static AdaptiveCard GenerateTicketsCard(IEnumerable<Vehicle> tickets)
        {
            AdaptiveCard card = new AdaptiveCard();
            if (tickets.Count() > 0)
            {
                var ticketCards = tickets.Select(x => new Container() { Separation = SeparationStyle.Strong, Items = GenerateTicketCardElements(x).ToList() });
                card.Body.AddRange(ticketCards);
            }
            else
                card = GenerateTextCard(TextFeedbackGenerator.MakeTicketsNotFoundFeedbackUntrivial());

            return card;
        }

        private static IEnumerable<CardElement> GenerateTicketCardElements(Vehicle ticket)
        {
            var ticketName = string.Format($"{ticket.Name} {ticket.Route}");
            var ticketTimes = string.Format($"Отправление: {ticket.DepartureTime}\t\tПрибытие: {ticket.ArrivalTime}");
            var cardElements = new List<CardElement>(){
                       CreateTextBlock(ticketName, TextSize.Large, TextWeight.Bolder, TextColor.Accent, HorizontalAlignment.Center),
                       CreateTextBlock(ticketTimes, TextSize.Medium, alignment:HorizontalAlignment.Center)};

            if (ticket.Places.Count() > 0)
            {
                var ticketPlacesInfo = ticket.Places.Select(x => string.Format($"{x.Type}\t\t{x.Cost}руб.\t\t{x.Amount}мест(а)"));
                var ticketPlaces = from place in ticketPlacesInfo select CreateTextBlock(place, TextSize.Medium, alignment: HorizontalAlignment.Center);
                cardElements.AddRange(ticketPlaces);
            }
            return cardElements;
        }

        public static AdaptiveCard GenerateTextCard(string text)
        {
            return new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    CreateTextBlock(text, TextSize.Large)
                }
            };
        }

        private static TextBlock CreateTextBlock(string text, TextSize size = TextSize.Normal, TextWeight weight = TextWeight.Normal, TextColor color = TextColor.Default, HorizontalAlignment alignment = HorizontalAlignment.Left)
        {
            return new TextBlock()
            {
                Text = text,
                Size = size,
                HorizontalAlignment = alignment,
                Weight = weight,
                Color = color
            };
        }
    }

}