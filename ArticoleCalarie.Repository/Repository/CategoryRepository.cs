using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }
    }
}
