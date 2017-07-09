using System.Collections.Generic;

namespace BestTickets.Domain.Abstractions
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
}
