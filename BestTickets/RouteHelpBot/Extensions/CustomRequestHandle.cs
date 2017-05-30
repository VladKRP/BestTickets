using AdaptiveCards;
using BestTickets.Extensions;
using BestTickets.Models;
using BestTickets.Services;
using Microsoft.Bot.Connector;
using RouteHelpBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteHelpBot.Extensions
{
    public static class  CustomRequestHandle
    {

        private static RouteViewModel RecognizeRoute(string activityText)
        {
            var departurePlace = activityText.Where((x, i) => i > activityText.IndexOf(" из ", StringComparison.CurrentCultureIgnoreCase) + 3 && i < activityText.IndexOf(" в ", StringComparison.CurrentCultureIgnoreCase) - 1)
                .TakeWhile(x => char.IsLetter(x)).Aggregate("", (x,y) => x+=y);
            var arrivalPlace = activityText.Where((x, i) => i > activityText.IndexOf(" в ", StringComparison.CurrentCultureIgnoreCase) + 2).TakeWhile(x => char.IsLetter(x)).Aggregate("", (x,y) => x+=y);           
            RouteViewModel route = new RouteViewModel(departurePlace, arrivalPlace, null);
            route.Date = route.SetCurrentDate();
            return route;      
        }
        
        private static string RecognizeVehicleKind(string activityText)
        {
            string vehicleKind = null;
            var isBus = activityText.IndexOf("автобус", StringComparison.CurrentCultureIgnoreCase) >= 0 || activityText.IndexOf("маршрутк",StringComparison.CurrentCultureIgnoreCase) >= 0;
            if (isBus)
                vehicleKind = "Маршрутка/Автобус";
            else
            {
                var isTrain = activityText.IndexOf("поезд", StringComparison.CurrentCultureIgnoreCase) >= 0 || activityText.IndexOf("электричк", StringComparison.CurrentCultureIgnoreCase) >= 0;
                if (isTrain)
                    vehicleKind = "Поезд/Электричка";
            }      
            return vehicleKind;
        }

        private static double? RecognizePrice(string activityText)
        {
            //not work yet
            double? returningValue = null;
            //var price = activityText.Where((x, i) => (i > activityText.IndexOf(" за ") + 3) || (i > activityText.IndexOf(" cтоимостью ")) || (i > activityText.IndexOf(" имея ")))
            //    .TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("", (x,y) => x+=y);
            var price = activityText.Where((x, i) => i > activityText.IndexOf(" за ") + 3).TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("", (x, y) => x += y);
            if (!string.IsNullOrEmpty(price))
                returningValue = double.Parse(price);
            return returningValue;
        }

        private static TimeSpan? RecognizeTime(string activityText)
        {
            TimeSpan? time = null;
            var findedTime = activityText.Where((x, i) => i > activityText.IndexOf(" на ", StringComparison.CurrentCultureIgnoreCase) + 3)
                .TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("",(x,y) => x+=y);
            if(!string.IsNullOrEmpty(findedTime))
            {
               var tempTime = findedTime.Split(':', '-','.');
               time = new TimeSpan(int.Parse(tempTime[0]), int.Parse(tempTime[1]), 0);
            }
            else
            {
                var isImmediateTicket = activityText.IndexOf("ближайш", StringComparison.CurrentCultureIgnoreCase) >= 0;
                if (isImmediateTicket)
                    time = DateTime.Now.TimeOfDay;
            }
            
            return time;
        }

        public static UserRequest RecognizeUserRequest(Activity activity)  
        {
            UserRequest recognizedUserRequest = new UserRequest()
            {
                Route = RecognizeRoute(activity.Text),
                Price = RecognizePrice(activity.Text),
                VehicleKind = RecognizeVehicleKind(activity.Text),
                Time = RecognizeTime(activity.Text)
            };
            return recognizedUserRequest;
        }

        public static string HandleTextRequest(UserRequest request)
        {
            string responseText;
            if (string.IsNullOrEmpty(request.Route.ArrivalPlace) || string.IsNullOrEmpty(request.Route.DeparturePlace))
                responseText = FeedbackGenerator.MakeTicketsNotFoundFeedbackUntrivial();
            else
            {
                var tickets = TicketChecker.GetByVehicleKind(request.Route, request.VehicleKind)
                    .GetTicketsByPrice(request.Price).GetTicketsByTimeOrNearest(request.Time);
                responseText = FeedbackGenerator.GenerateTicketsFeedbackMessage(tickets);
            }         
            return responseText;
        }
    
    }
}