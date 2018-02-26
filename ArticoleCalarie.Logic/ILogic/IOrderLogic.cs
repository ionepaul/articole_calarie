using System.Threading.Tasks;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IOrderLogic
    {
        Task<int> PlaceOrder(OrderViewModel checkoutViewModel, string userId);
        Task<OrderSearchViewResult> GetOrders(int pageNumber, OrderStatusViewEnum status);
        Task ChangeOrderStatus(int orderNumber, OrderStatusViewEnum newOrderStatus, string deliveryTime);
        Task<OrderSearchViewResult> GetUserOrders(int pageNumber, string userId);
    }
}
