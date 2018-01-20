using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IAbstractRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
        Task AddAsync(T entity);
        void Add(T entity);
        Task SaveChanges();
        Task Delete(T entity);
    }
}
