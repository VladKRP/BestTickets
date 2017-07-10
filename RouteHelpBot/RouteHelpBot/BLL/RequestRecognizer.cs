using BestTickets.Domain.Models;
using Microsoft.Bot.Connector;
using RouteHelpBot.DAL;
using RouteHelpBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteHelpBot.BLL
{
    public class RequestRecognizer
    {
        public static UserRequest RecognizeUserRequest(Activity activity)
        {
            UserRequest recognizedUserRequest = null;
            var messageText = activity.Text;
            if (messageText != null)
            {
                recognizedUserRequest = new UserRequest()
                {
                    KeyWord = RecognizeGreeting(messageText),
                    Route = RecognizeRoute(messageText),
                    Price = RecognizePrice(messageText),
                    VehicleKind = RecognizeVehicleKind(messageText),
                    Time = RecognizeTime(messageText)
                };
            }     
            return recognizedUserRequest;
        }

        private static Route RecognizeRoute(string messageText)
        {
            var route = RecognizeRouteByKeywords(messageText);
            if (string.IsNullOrEmpty(route.ArrivalPlace) || string.IsNullOrEmpty(route.DeparturePlace))
                route = RecognizeRouteWithDB(messageText);
            return route;
        }

        private static Route RecognizeRouteByKeywords(string messageText)
        {
            var departurePlace = messageText.Where((x, i) => i > messageText.IndexOf(" из ", StringComparison.CurrentCultureIgnoreCase) + 3 && i < messageText.IndexOf(" в ", StringComparison.CurrentCultureIgnoreCase) - 1)
                                             .TakeWhile(x => char.IsLetter(x)).Aggregate("", (x, y) => x += y);
            var arrivalPlace = messageText.Where((x, i) => i > messageText.IndexOf(" в ", StringComparison.CurrentCultureIgnoreCase) + 2)
                                           .TakeWhile(x => char.IsLetter(x)).Aggregate("", (x, y) => x += y);
            Route route = new Route(departurePlace, arrivalPlace, DateTime.Now);
            return route;
        }

        private static Route RecognizeRouteWithDB(string messageText)//To change
        {
            Route route = new Route(null, null, DateTime.Now);
            var requestText = messageText.Split(' ', ',', '-', ';');
            var cities = new CitiesDictionary().Cities;
            foreach (var word in requestText)
            {
                var city = cities.Where(x => word.IndexOf(x.Value, StringComparison.CurrentCultureIgnoreCase) >= 0).Select(x => x.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(city))
                {
                    if (route.DeparturePlace == null)
                        route.DeparturePlace = city;
                    else if (route.ArrivalPlace == null)
                        route.ArrivalPlace = city;
                    else
                        break;
                }
            }
            return route;
        }

        private static string RecognizeVehicleKind(string messageText)
        {
            string vehicleKind = null;
            if (IsBus(messageText))
                vehicleKind = "Маршрутка/Автобус";
            else if (IsTrain(messageText))
                vehicleKind = "Поезд/Электричка";
            return vehicleKind;
        }

        private static bool IsBus(string text)
        {
            bool IsBus = false;
            if (text.IndexOf("автобус", StringComparison.CurrentCultureIgnoreCase) >= 0 || text.IndexOf("маршрутк", StringComparison.CurrentCultureIgnoreCase) >= 0)
                IsBus = true;
            return IsBus;
        }

        private static bool IsTrain(string text)
        {
            bool IsTrain = false;
            if (text.IndexOf("поезд", StringComparison.CurrentCultureIgnoreCase) >= 0 || text.IndexOf("электричк", StringComparison.CurrentCultureIgnoreCase) >= 0)
                IsTrain = true;
            return IsTrain;
        }

        private static double? RecognizePrice(string messageText)//To change
        {
            var recognizedPhrases = new List<string>() { " за ", " имея ", " стоимостью " };
            double? returningValue = null;
            var searchPhrase = recognizedPhrases.Where(x => messageText.IndexOf(x, StringComparison.CurrentCultureIgnoreCase) > 0).FirstOrDefault();
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                var price = messageText.Where((x, i) => i > messageText.IndexOf(searchPhrase) + searchPhrase.Length - 1)
                                        .TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("", (x, y) => x += y);
                if (!string.IsNullOrEmpty(price))
                {
                    if (price.Contains(','))
                        price = price.Replace(',', '.');
                    returningValue = double.Parse(price);
                }      
            }
            return returningValue;
        }

        private static TimeSpan? RecognizeTime(string messageText)
        {
            TimeSpan? time = null;
            var findedTime = messageText.Where((x, i) => i > messageText.IndexOf(" на ", StringComparison.CurrentCultureIgnoreCase) + 3)
                .TakeWhile(x => char.IsDigit(x) || char.IsPunctuation(x)).Aggregate("", (x, y) => x += y);
            if (!string.IsNullOrEmpty(findedTime))
                time = ProcessTime(findedTime);
            else if (IsImmediateTicketRequested(messageText))
                time = DateTime.Now.TimeOfDay;
            return time;
        }

        private static bool IsImmediateTicketRequested(string text)
        {
            bool isImmediateTicketRequested = false;
            if (text.IndexOf("ближайш", StringComparison.CurrentCultureIgnoreCase) >= 0)
                isImmediateTicketRequested = true;
            return isImmediateTicketRequested;
        }

        private static TimeSpan? ProcessTime(string foundedTime)
        {
            TimeSpan? time = null;
            var tempTime = foundedTime.Split(':', '-');
            if (tempTime.Length > 1)
                time = new TimeSpan(int.Parse(tempTime[0]), int.Parse(tempTime[1]), 0);
            else
                time = new TimeSpan(int.Parse(tempTime[0]), 0, 0);
            return time;
        }

        private static string RecognizeGreeting(string messageText)
        {
            string greeting = null;
            if (IsAnyGreeatingPhraseRecognized(messageText))
                greeting = "Приветствие";
            return greeting;
        }

        private static bool IsAnyGreeatingPhraseRecognized(string text)
        {
            bool isAnyGreetingRecognized = false;
            var greetingPhrases = new List<string>() { "Привет", "Здравствуй", "Добрый день", "Хай", "Ку", "Добрый вечер" };
            if (greetingPhrases.Count(x => text.IndexOf(x, StringComparison.CurrentCultureIgnoreCase) >= 0) > 0)
                isAnyGreetingRecognized = true;
            return isAnyGreetingRecognized;
        }
    }
}