using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using BestTickets.Models;
using BestTickets.Controllers;
using System.Text;
using BestTickets.Services;
using System.Collections.Generic;

namespace RouteHelpBot
{
    //[BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                var feedback = HandleUserRequest(activity);

                Activity reply = activity.CreateReply(feedback);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                 HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private string HandleUserRequest(Activity activity)
        {
            var request = activity.Text;
            var departurePlace = request.Where((x, i) => i > request.IndexOf("из") + 2 && i < request.IndexOf("в") - 2);
            var arrivalPlace = request.Where((x, i) => i > request.IndexOf("в") + 1);
            departurePlace = string.Join("", departurePlace);
            arrivalPlace = string.Join("", arrivalPlace);
            RouteViewModel route = new RouteViewModel(departurePlace.ToString(), arrivalPlace.ToString(), null);
            var tickets = new TicketsController().GetTickets(route);
            return GenerateFeedbackMessage(tickets); 
        }

        private string GenerateFeedbackMessage(IEnumerable<Vehicle> tickets)
        {
            var feedbackMessage = new StringBuilder();
            if (tickets.Count() > 0)
            {
                feedbackMessage.Append($"Вот что я нашел по вашему запросу:\n ");
                foreach (var ticket in tickets)
                {

                    feedbackMessage.Append($"---\n\r{ticket.Name} {ticket.Type}\n\rОтправление:{ticket.DepartureTime}\n\rПрибытие:{ticket.ArrivalTime}\n\r");
                    if (ticket.Places.Count() > 0)
                    {
                        feedbackMessage.Append($"Места:\n\r");
                        foreach (var place in ticket.Places)
                            feedbackMessage.Append($"{place.Type}/{place.Amount}/{place.Cost} руб.\n\r");
                    }

                }
            }
            else
                feedbackMessage.Append(MakeNotFoundFeedbackUntrivial());
            return feedbackMessage.ToString();
        }

        private string MakeNotFoundFeedbackUntrivial()
        {
            var feedback = new List<string>() { "Билетов по вашему запросу не найденно.Возможно вы указали что-то не верно.",
                "Мне не удалось ничего найти. Возможно что-то указано не верно.",
                "Возможно вы в чем-то ошиблись. Мне не удалось ничего найти.",
           "По вашему запросу ничего не найдено. Возможно вы что-то ввели неверно." };
            return feedback.ElementAt(new Random().Next(feedback.Count()));
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}