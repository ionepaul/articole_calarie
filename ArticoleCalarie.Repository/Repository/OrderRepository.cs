using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Enums;
using ArticoleCalarie.Repository.IRepository;
using ArticoleCalarie.Repository.Models;

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

        public async Task<OrderSearchResult> GetAllOrders(int itemsPerPage, int itemsToSkip)
        {
            var ordersResult = new OrderSearchResult
            {
                TotalCount = await _dbset.CountAsync(),
                Orders = await _dbset.OrderBy(x => x.OrderRegistrationDate).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
            };

            return ordersResult;
        }

        public async Task<OrderSearchResult> GetOrdersByStatus(int itemsPerPage, int itemsToSkip, OrderStatus status)
        {
            IQueryable<Order> query = null;

            switch (status)
            {
                case OrderStatus.COMPLETE:
                    query = _dbset.Where(x => x.OrderStatus == OrderStatus.COMPLETE);
                    break;
                case OrderStatus.CONFIRMED:
                    query = _dbset.Where(x => x.OrderStatus == OrderStatus.CONFIRMED);
                    break;
                case OrderStatus.REGISTRED:
                    query = _dbset.Where(x => x.OrderStatus == OrderStatus.REGISTRED);
                    break;
                case OrderStatus.SHIPPED:
                    query = _dbset.Where(x => x.OrderStatus == OrderStatus.SHIPPED);
                    break;
            }

            if (query != null)
            {
                var ordersResult = new OrderSearchResult
                {
                    TotalCount = await query.CountAsync(),
                    Orders = await query.OrderBy(x => x.OrderRegistrationDate).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
                };

                return ordersResult;
            }

            return null;
        }
    }
}
