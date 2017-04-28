
namespace BestTickets.Models
{
    public class RouteRequest
    {
        public int Id { get; set; }
        public RouteViewModel Route { get; set; }
        public int RequestsCount { get; set; } = 1;
    }
}