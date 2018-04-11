using System;
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
            var query = _dbset.Include(x => x.DeliveryAddress).Include(x => x.BillingAddress).Include(x => x.OrderItems);

            var ordersResult = new OrderSearchResult
            {
                TotalCount = await query.CountAsync(),
                Orders = await query.OrderBy(x => x.OrderRegistrationDate).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
            };

            return ordersResult;
        }

        public async Task<Order> GetOrderByOrderNumber(int orderNumber)
        {
            var order = await _dbset.Include(x => x.OrderItems).Include(x => x.DeliveryAddress).FirstAsync(x => x.OrderNumber == orderNumber);

            return order;
        }

        public async Task<OrderSearchResult> GetOrdersByStatus(int itemsPerPage, int itemsToSkip, OrderStatus status)
        {
            var query = _dbset.Include(x => x.DeliveryAddress).Include(x => x.BillingAddress).Include(x => x.OrderItems);

            switch (status)
            {
                case OrderStatus.COMPLETE:
                    query = query.Where(x => x.OrderStatus == OrderStatus.COMPLETE);
                    break;
                case OrderStatus.CONFIRMED:
                    query = query.Where(x => x.OrderStatus == OrderStatus.CONFIRMED);
                    break;
                case OrderStatus.REGISTRED:
                    query = query.Where(x => x.OrderStatus == OrderStatus.REGISTRED);
                    break;
                case OrderStatus.SHIPPED:
                    query = query.Where(x => x.OrderStatus == OrderStatus.SHIPPED);
                    break;
            }

            var ordersResult = new OrderSearchResult
            {
                TotalCount = await query.CountAsync(),
                Orders = await query.OrderByDescending(x => x.OrderRegistrationDate).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
            };

            return ordersResult;
        }

        public async Task<OrderSearchResult> GetUserOrders(int itemsPerPage, int itemsToSkip, string userId)
        {
            var query = _dbset.Include(x => x.DeliveryAddress).Include(x => x.BillingAddress).Include(x => x.OrderItems)
                              .Where(x => string.Equals(x.UserId, userId));

            var ordersResult = new OrderSearchResult
            {
                TotalCount = await query.CountAsync(),
                Orders = await query.OrderByDescending(x => x.OrderRegistrationDate).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
            };

            return ordersResult;
        }

        public async Task UpdateOrder(Order order)
        {
            _ctx.Entry(order).State = EntityState.Modified;

            await SaveChangesAsync();
        }
    }
}
