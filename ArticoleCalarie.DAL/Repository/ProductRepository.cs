using ArticoleCalarie.Repository;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;
using ArticoleCalarie.Repository.Repository;

namespace ArticoleCalarie.DAL.Repository
{
    public class ProductRepository : AbstractRepository<Product>, IProductRepository
    {
        public ProductRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }
    }
}
