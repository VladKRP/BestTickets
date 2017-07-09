<<<<<<< HEAD:BestTickets.Web/RouteHelpBot/Model/UserRequest.cs
﻿using BestTickets.Domain.Models;
using System;
=======
﻿using System;
using BestTickets.Domain.Models;
>>>>>>> eb381e900aba17531816f3127b793c6acebddf49:BestTickets/RouteHelpBot/Model/UserRequest.cs

namespace RouteHelpBot.Model
{
    public class UserRequest
    {
        public Route Route { get; set; }
        public string VehicleKind { get; set; } = null;
        public TimeSpan? Time { get; set; } = null;
        public double? Price { get; set; } = null;

        public string KeyWord { get; set; } = null;

        public UserRequest()
        {
            Route = null;
        }

        public UserRequest(Route route, string vehicleKind = null, TimeSpan? time = null, double? price = null, string keyWord = null)
        {
            Route = route;
            VehicleKind = vehicleKind;
            Time = time;
            Price = price;
            KeyWord = keyWord;
        }
    }
}