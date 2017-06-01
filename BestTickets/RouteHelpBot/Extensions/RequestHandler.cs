using AdaptiveCards;
using BestTickets.Extensions;
using BestTickets.Services;
using RouteHelpBot.Model;

namespace RouteHelpBot.Extensions
{
    public class RequestHandler
    {
        public static AdaptiveCard HandleRequestAsAdaptiveCard(UserRequest request)
        {
            AdaptiveCard card = new AdaptiveCard();
            if (string.IsNullOrEmpty(request.Route.ArrivalPlace) || string.IsNullOrEmpty(request.Route.DeparturePlace))
            { 
                if (request.KeyWord == "Приветствие")
                    card = AdaptiveCardFeedbackGenerator.GenerateGreetingCard();
                else
                    card = AdaptiveCardFeedbackGenerator.GenerateWrongRouteCard();
            }
            else
            {
                var tickets = TicketChecker.GetByVehicleKind(request.Route, request.VehicleKind).GetTicketsByPrice(request.Price).GetTicketsByTimeOrNearest(request.Time);
                card = AdaptiveCardFeedbackGenerator.GenerateTicketsCard(tickets);
            }

            return card;
        }

        public static string HandleRequestAsText(UserRequest request)
        {
            string responseText;
            if (string.IsNullOrEmpty(request.Route.ArrivalPlace) || string.IsNullOrEmpty(request.Route.DeparturePlace))
                responseText = TextFeedbackGenerator.MakeWrongRouteFeedbackUntrivial();
            else
            {
                var tickets = TicketChecker.GetByVehicleKind(request.Route, request.VehicleKind).GetTicketsByPrice(request.Price).GetTicketsByTimeOrNearest(request.Time);
                responseText = TextFeedbackGenerator.GenerateTicketsFeedbackMessage(tickets);
            }
            return responseText;
        }
    }
}