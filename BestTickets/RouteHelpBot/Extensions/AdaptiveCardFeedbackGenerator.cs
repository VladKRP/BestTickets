using AdaptiveCards;
using BestTickets.Models;
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
                foreach (var ticket in tickets)
                {
                    var ticketName = string.Format($"{ticket.Name} {ticket.Route}");
                    var ticketTimes = string.Format($"Отправление: {ticket.DepartureTime}\t\tПрибытие: {ticket.ArrivalTime}");     
                    var cardElements = new List<CardElement>(){
                       CreateTextBlock(ticketName, TextSize.Large, TextWeight.Bolder, TextColor.Accent, HorizontalAlignment.Center),
                       CreateTextBlock(ticketTimes, TextSize.Medium, alignment:HorizontalAlignment.Center)};
                    if(ticket.Places.Count() > 0)
                    {
                        var ticketPlacesInfo = ticket.Places.Select(x => string.Format($"{x.Type}\t\t{x.Cost}руб.\t\t{x.Amount}мест(а)"));
                        foreach (var place in ticketPlacesInfo)
                            cardElements.Add(CreateTextBlock(place, TextSize.Medium, alignment: HorizontalAlignment.Center));
                    }
                    card.Body.Add(new Container()
                    {
                        Separation = SeparationStyle.Strong,
                        Items = cardElements
                    });
                }
            }
            else
                card = GenerateTicketsNotFoundCard();
            
            return card;
        }

        public static AdaptiveCard GenerateTicketsNotFoundCard()
        {
            return new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    CreateTextBlock(TextFeedbackGenerator.MakeTicketsNotFoundFeedbackUntrivial(), TextSize.Large)
                }
            };
        }

        public static AdaptiveCard GenerateWrongRouteCard()
        {
            return new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    CreateTextBlock(TextFeedbackGenerator.MakeWrongRouteFeedbackUntrivial(), TextSize.Large)
                }
            };
        }

        public static AdaptiveCard GenerateGreetingCard()
        {
            return new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    CreateTextBlock(TextFeedbackGenerator.MakeGreetingFeedbackUntrivial(), TextSize.Large)
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