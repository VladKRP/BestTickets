namespace BestTickets.Domain.Models
{
    public class RouteRequest
    {
        public int Id { get; set; }
        public Route Route { get; set; }
        public int RequestsCount { get; set; } = 0;
    }
}