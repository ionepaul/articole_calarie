using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class BrandRepository : AbstractRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Brand> GetBrandsBySearchTerm(string searchTerm)
        {
            var brands = _dbset.Where(x => x.Name.StartsWith(searchTerm)).AsEnumerable();

            return brands;
        }
    }
}
