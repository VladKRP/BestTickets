using System.Collections.Generic;

namespace BestTickets.Models
{
    public class Vehicle
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Route { get; set; }
        
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public IEnumerable<VehiclePlace> Places { get; set; }      
    }
}
