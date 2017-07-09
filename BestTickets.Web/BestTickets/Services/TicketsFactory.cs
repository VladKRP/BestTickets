
namespace BestTickets.Services
{
    public class TicketsFactory
    {
        
        public ITicketsFinder GetTicketFinder(string vehicleKind)
        {
            ITicketsFinder TicketFinder = null;

            if(string.IsNullOrEmpty(vehicleKind))
                TicketFinder = new MultipleTicketsFinder();
            else if (vehicleKind.Equals("Маршрутка/Автобус"))
                TicketFinder = new TicketBusTicketsFinder();
            else if (vehicleKind.Equals("Поезд/Электричка"))
                TicketFinder = new RaspRwTicketsFinder();
            return TicketFinder;
        }
    }
}