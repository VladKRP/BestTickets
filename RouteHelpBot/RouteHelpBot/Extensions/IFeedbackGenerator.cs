using BestTickets.Domain.Models;
using System.Collections.Generic;

namespace RouteHelpBot.Extensions
{
    public interface IFeedbackGenerator<T>
    {
        T GenerateFeedback(IEnumerable<Vehicle> tickets);
    }
}
