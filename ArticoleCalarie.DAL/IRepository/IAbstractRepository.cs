using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IAbstractRepository<T>
    {
        Task<T> GetById(int id);
        IEnumerable<T> GetAll();
        Task Add(T entity);
        Task SaveChanges();
        Task Delete(T entity);
    }
}
