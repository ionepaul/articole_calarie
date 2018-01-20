using System.Collections.Generic;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface ICategoryRepository : IAbstractRepository<Category>
    {
        IEnumerable<Category> GetCategoriesBySearchTerm(string searchTerm);
    }
}
