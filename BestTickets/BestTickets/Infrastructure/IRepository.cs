using BestTickets.Models;
using System.Collections.Generic;

namespace BestTickets.Infrastructure
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T FindById(int id);
        T FindByRoute(RouteViewModel route);
        void Create(T route);
        void Update(T route);
        void Delete(int id);
        void Save();
    }
}
