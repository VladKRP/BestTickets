using RouteHelpBot.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace RouteHelpBot.DAL
{
    public class DapperCityRepository:ICityRepository
    {
        private readonly string connectionString;

        public DapperCityRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<City> GetAll()
        {
            IEnumerable<City> cities = Enumerable.Empty<City>();
            using (IDbConnection db = new SqlConnection(connectionString))
                cities = db.Query<City>("SELECT * FROM Cities").ToList();
            return cities;
        }
    }
}