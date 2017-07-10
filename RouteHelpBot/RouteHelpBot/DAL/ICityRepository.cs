using RouteHelpBot.Model;
using System.Collections.Generic;

namespace RouteHelpBot.DAL
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAll();
    }
}
