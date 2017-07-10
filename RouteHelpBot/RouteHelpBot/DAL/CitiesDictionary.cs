using RouteHelpBot.Model;
using System.Collections.Generic;

namespace RouteHelpBot.DAL
{
    public class CitiesDictionary
    {
        public Dictionary<int, string> Cities { get; private set; }
        private readonly DapperCityRepository repo = 
            new DapperCityRepository(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Workspace\BestTickets\RouteHelpBot\RouteHelpBot\App_Data\CitiesHandbook.mdf;Integrated Security=True");
       
        public CitiesDictionary()
        {
            Cities = GenerateCitiesDictionary(repo.GetAll());
        }

        private static Dictionary<int, string> GenerateCitiesDictionary(IEnumerable<City> cities)
        {
            Dictionary<int, string> cityDictionary = new Dictionary<int, string>();
            foreach (var city in cities)
                cityDictionary.Add(city.Id, city.Name.Trim());
            return cityDictionary;

        }
    }
}