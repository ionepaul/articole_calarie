using System.Collections.Generic;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IBrandRepository : IAbstractRepository<Brand>
    {
        IEnumerable<Brand> GetBrandsBySearchTerm(string searchTerm);
    }
}
