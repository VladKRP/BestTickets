
namespace BestTickets.Services
{
    public class TicketsFactory
    {
        public ITicketsFinder TicketFinder { get; private set; }

        public ITicketsFinder GetTicketsByVehicleKind(string vehicleKind)
        {
            if (vehicleKind.Equals("Маршрутка/Автобус"))
                TicketFinder = new TicketBusTicketsFinder();
            else if (vehicleKind.Equals("Поезд/Электричка"))
                TicketFinder = new RaspRwTicketsFinder();
            else
                TicketFinder = new MultipleTicketsFinder();
            return TicketFinder;
        }
    }
}