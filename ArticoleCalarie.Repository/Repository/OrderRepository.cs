using System.Data.Entity;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class OrderRepository : AbstractRepository<Order>, IOrderRepository
    {
        public OrderRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }

        public async Task<int> CountOrders()
        {
            return await _dbset.CountAsync();
        }
    }
}
