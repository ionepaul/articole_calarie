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
        Task SaveChangesAsync();
        void SaveChanges();
        Task DeleteAsync(T entity);
        void Delete(T entity);
    }
}
