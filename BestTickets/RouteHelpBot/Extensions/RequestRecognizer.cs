using BestTickets.Models;
using Microsoft.Bot.Connector;
using RouteHelpBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteHelpBot.Extensions
{
    public static class  RequestRecognizer
    {
        public static UserRequest RecognizeUserRequest(Activity activity)
        {
            UserRequest recognizedUserRequest = new UserRequest()
            {
                Route = RecognizeRoute(activity.Text),
                Price = RecognizePrice(activity.Text),
                VehicleKind = RecognizeVehicleKind(activity.Text),
                Time = RecognizeTime(activity.Text),
                KeyWord = RecognizeGreeting(activity.Text)
            };
            return recognizedUserRequest;
        }

        private static RouteViewModel RecognizeRoute(string activityText)
        {
            var departurePlace = activityText.Where((x, i) => i > activityText.IndexOf(" из ", StringComparison.CurrentCultureIgnoreCase) + 3 && i < activityText.IndexOf(" в ", StringComparison.CurrentCultureIgnoreCase) - 1)
                .TakeWhile(x => char.IsLetter(x)).Aggregate("", (x,y) => x+=y);
            var arrivalPlace = activityText.Where((x, i) => i > activityText.IndexOf(" в ", StringComparison.CurrentCultureIgnoreCase) + 2).TakeWhile(x => char.IsLetter(x)).Aggregate("", (x,y) => x+=y);           
            RouteViewModel route = new RouteViewModel(departurePlace, arrivalPlace, null);
            route.Date = route.SetCurrentDate();
            return route;      
        }

        private static RouteViewModel RecognizeRouteDbIdentification(string activityText)
        {
            var context = new RouteHelpBot.DAL.CitiesContext();
            var city = context.Cities.Where(x => x.Name == "Брест");
            var requestText = activityText.Split(' ');
            return null;
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
            var recognizedPhrases = new List<string>() { " за ", " имея ", " стоимостью " };
            double? returningValue = null;
            var searchPhrase = recognizedPhrases.Where(x => activityText.IndexOf(x, StringComparison.CurrentCultureIgnoreCase) > 0).FirstOrDefault();
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                var price = activityText.Where((x, i) => i > activityText.IndexOf(searchPhrase) + searchPhrase.Length - 1).TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("", (x, y) => x += y);
                if (price.Contains(','))
                    price = price.Replace(',', '.');
                if (!string.IsNullOrEmpty(price))
                    returningValue = double.Parse(price);
            }   
            return returningValue;
        }

        private static TimeSpan? RecognizeTime(string activityText)
        {
            TimeSpan? time = null;
            var findedTime = activityText.Where((x, i) => i > activityText.IndexOf(" на ", StringComparison.CurrentCultureIgnoreCase) + 3)
                .TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("",(x,y) => x+=y);
            if (!string.IsNullOrEmpty(findedTime))
                time = ProcessTime(findedTime);
            else
            {
                var isImmediateTicket = activityText.IndexOf("ближайш", StringComparison.CurrentCultureIgnoreCase) >= 0;
                if (isImmediateTicket)
                    time = DateTime.Now.TimeOfDay;
            }
            return time;
        }

        private static string RecognizeGreeting(string activityText)
        {
            string greeting = null;
            var greetingPhrases = new List<string>() { "Привет", "Здравствуй", "Добрый день", "Хай", "Ку", "Добрый вечер" };
            var greetingPhrasesInRequest = greetingPhrases.Where(x => activityText.IndexOf(x, StringComparison.CurrentCultureIgnoreCase) >= 0);
            if (greetingPhrasesInRequest.Count() > 0)
                greeting = "Приветствие";
            return greeting;
        }

        private static TimeSpan? ProcessTime(string findedTime)
        {
            TimeSpan? time = null;
            var tempTime = findedTime.Split(':', '-', '.');
            if (tempTime.Length > 1)
                time = new TimeSpan(int.Parse(tempTime[0]), int.Parse(tempTime[1]), 0);
            else
                time = new TimeSpan(int.Parse(tempTime[0]), 0, 0);
            return time;
        }
    }
}