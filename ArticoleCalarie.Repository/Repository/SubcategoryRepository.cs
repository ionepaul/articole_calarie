using System.Collections.Generic;
using System.Data.Entity;
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
            var subcategories = _dbset.Where(x => x.CategoryId == categoryId && x.Products.Count > 0).Include(x => x.Category).OrderBy(x => x.Name).AsEnumerable();

            return subcategories;
        }

        public IEnumerable<Subcategory> GetAllByCategoryIdAndSearchTerm(int categoryId, string searchTerm = "")
        {
            var subcategories = _dbset.Where(x => x.CategoryId == categoryId && x.Name.StartsWith(searchTerm)).AsEnumerable();

            return subcategories;
        }

        public IEnumerable<Subcategory> GetAllByCategoryName(string categoryName)
        {
            var subcategories = _dbset.Include(x => x.Category).Where(x => x.Category.Name.ToLower() == categoryName.ToLower()).AsEnumerable();

            return subcategories;
        }

        public override IEnumerable<Subcategory> GetAll()
        {
            var subcategories = _dbset.Include(x => x.Category).Include(x => x.Products).AsEnumerable();

            return subcategories;
        }
    }
}
