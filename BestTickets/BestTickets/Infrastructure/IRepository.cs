using BestTickets.Models;
using System.Collections.Generic;
using System.Linq;

namespace BestTickets.Infrastructure
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        IQueryable<T> GetTop10();
        T FindById(int id);
        T FindByRoute(RouteViewModel route);
        void Create(T route);
        void Update(T route);
        void Delete(int id);
        void Save();
    }
}
