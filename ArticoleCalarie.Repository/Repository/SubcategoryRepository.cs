using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class SubcategoryRepository : AbstractRepository<Subcategory>, ISubcategoryRepository
    {
        public SubcategoryRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Subcategory> GetAllByCategoryId(int categoryId)
        {
            var subcategories = _dbset.Where(x => x.CategoryId == categoryId).AsEnumerable();

            return subcategories;
        }

        public IEnumerable<Subcategory> GetAllByCategoryIdAndSearchTerm(int categoryId, string searchTerm = "")
        {
            var subcategories = _dbset.Where(x => x.CategoryId == categoryId && x.Name.StartsWith(searchTerm)).AsEnumerable();

            return subcategories;
        }
    }
}
