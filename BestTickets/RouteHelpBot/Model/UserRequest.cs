using BestTickets.Models;
using System;

namespace RouteHelpBot.Model
{
    public class UserRequest
    {
        public RouteViewModel Route { get; set; }
        public string VehicleKind { get; set; } = null;
        public TimeSpan? Time { get; set; } = null;
        public double? Price { get; set; } = null; 

        public UserRequest()
        {
            Route = null;
        }

        public UserRequest(RouteViewModel route, string vehicleKind = null, TimeSpan? time = null, double? price = null)
        {
            Route = route;
            VehicleKind = vehicleKind;
            Time = time;
            Price = price;
        }
    }
}