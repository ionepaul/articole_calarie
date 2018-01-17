using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Category> GetCategories(string term)
        {
            var categories = _dbset.Where(x => x.Name.StartsWith(term)).AsEnumerable();

            return categories;
        }
    }
}
