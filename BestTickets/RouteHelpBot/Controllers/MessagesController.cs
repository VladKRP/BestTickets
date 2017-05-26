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
                var request = activity.Text;
                var departurePlace = request.Where((x,i) => i > request.IndexOf("из") + 2 && i < request.IndexOf("в") - 1);
                var arrivalPlace = request.Where((x,i) => i > request.IndexOf("в"));
                departurePlace = string.Join("", departurePlace);
                arrivalPlace = string.Join("", arrivalPlace);
                RouteViewModel route = new RouteViewModel(departurePlace.ToString(), arrivalPlace.ToString(), null);

                var controller = new TicketsController();

                var tickets = controller.GetTickets(route);


                var replyMessage = new StringBuilder();
                if (tickets.Count() > 0)
                {
                    replyMessage.Append($"Билеты по маршруту {arrivalPlace} - {departurePlace} :\n ");
                    foreach (var ticket in tickets)
                    {
                        replyMessage.Append($"---\nНазвание транспорта:{ticket.Name}\nОтправление:{ticket.DepartureTime}\nПрибытие:{ticket.ArrivalTime}\nМеста:{ticket.Places}\n---\n");
                    }
                }
                else
                    replyMessage.Append($"Билетов по вашему запросу не найденно.Возможно вы указали что-то не верно.");

                Activity reply = activity.CreateReply(replyMessage.ToString());
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
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