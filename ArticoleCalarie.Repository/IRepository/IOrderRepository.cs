using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Enums;
using ArticoleCalarie.Repository.Models;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IOrderRepository : IAbstractRepository<Order>
    {
        Task<int> CountOrders();
        Task<OrderSearchResult> GetAllOrders(int itemsPerPage, int itemsToSkip);
        Task<OrderSearchResult> GetOrdersByStatus(int itemsPerPage, int itemsToSkip, OrderStatus status);
        Task UpdateOrder(Order order);
        Task<Order> GetOrderByOrderNumber(int orderNumber);
        Task<OrderSearchResult> GetUserOrders(int itemsPerPage, int itemsToSkip, string userId);
    }
}
