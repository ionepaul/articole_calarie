using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Constants;
using ArticoleCalarie.Models.Enums;
using ArticoleCalarie.Repository.IRepository;
using ArticoleCalarie.Repository.Models;

namespace ArticoleCalarie.Logic.Logic
{
    public class OrderLogic : IOrderLogic
    {
        private static IOrderRepository _iOrderRepository;
        private static IAccountRepository _iAccountRepository;
        private static IEmailLogic _iEmailLogic;

        public OrderLogic(IOrderRepository iOrderRepository, IAccountRepository iAccountRepository, IEmailLogic iEmailLogic)
        {
            _iOrderRepository = iOrderRepository;
            _iAccountRepository = iAccountRepository;
            _iEmailLogic = iEmailLogic;
        }

        public async Task ChangeOrderStatus(int orderNumber, OrderStatusViewEnum newOrderStatus, string deliveryTime)
        {
            var order = await _iOrderRepository.GetOrderByOrderNumber(orderNumber);

            if (order == null)
            {
                throw new Exception("Order could not be found.");
            }

            order.OrderStatus = newOrderStatus.ToDbEnum();

            await _iOrderRepository.UpdateOrder(order);

            switch (newOrderStatus)
            {
                case OrderStatusViewEnum.CONFIRMED:
                    await _iEmailLogic.SendConfirmationOrderEmail(order);
                    break;
                case OrderStatusViewEnum.SHIPPED:
                    await _iEmailLogic.SendShippedOrderEmail(order, deliveryTime);
                    break;
            }
        }

        public async Task<OrderSearchViewResult> GetOrders(int pageNumber, OrderStatusViewEnum status)
        {
            var itemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["AdminOrdersPerPage"]);
            var itemsToSkip = (pageNumber - 1) * itemsPerPage;
            var dbStatus = status.ToDbEnum();

            OrderSearchResult result = null;

            if (status == OrderStatusViewEnum.ALL)
            {
                result = await _iOrderRepository.GetAllOrders(itemsPerPage, itemsToSkip);
            }
            else
            {
                result = await _iOrderRepository.GetOrdersByStatus(itemsPerPage, itemsToSkip, dbStatus);
            }

            if (result != null)
            {
                var orderSearchViewResult = new OrderSearchViewResult
                {
                    TotalCount = result.TotalCount,
                    Orders = result.Orders.Select(x => x.ToViewModel())
                };

                return orderSearchViewResult;
            }

            return null;
        }

        public async Task<OrderSearchViewResult> GetUserOrders(int pageNumber, string userId)
        {
            var itemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["UserOrdersPerPage"]);
            var itemsToSkip = (pageNumber - 1) * itemsPerPage;

            var userOrders = await _iOrderRepository.GetUserOrders(itemsPerPage, itemsToSkip, userId);

            var orderSearchViewResult = new OrderSearchViewResult
            {
                TotalCount = userOrders.TotalCount,
                Orders = userOrders.Orders.Select(x => x.ToViewModel())
            };

            return orderSearchViewResult;
        }

        public async Task<int> PlaceOrder(OrderViewModel orderViewModel, string userId)
        {
            var orderModel = orderViewModel.ToDbModel();

            if (!string.IsNullOrEmpty(userId))
            {
                orderModel.UserId = userId;

                var user = await _iAccountRepository.GetUserFullUserInfo(userId);

                if (user == null)
                {
                    orderModel.DeliveryAddress = orderViewModel.DeliveryAddress.ToDbAddress();
                    orderModel.BillingAddress = orderViewModel.BillingAddress.ToDbAddress();
                }
                else
                {
                    orderModel.DeliveryAddressId = user.DeliveryAddressId.HasValue ? user.DeliveryAddressId.Value : -1;
                    orderModel.BillingAddressId = user.BillingAddressId.HasValue ? user.BillingAddressId.Value : orderModel.DeliveryAddressId;
                    orderModel.Email = user.Email;
                }
            }
            else
            {
                orderModel.DeliveryAddress = orderViewModel.DeliveryAddress.ToDbAddress();
                orderModel.BillingAddress = orderViewModel.BillingAddress.ToDbAddress();
            }

            var totalOrders = await _iOrderRepository.CountOrders();

            orderModel.OrderNumber = ApplicationConstants.StartingOrderNumber + totalOrders;
            orderModel.OrderRegistrationDate = DateTime.UtcNow;

            var tasks = new List<Task>();

            var addOrder = _iOrderRepository.AddAsync(orderModel);
            tasks.Add(addOrder);

            var sendEmail = _iEmailLogic.SendNewOrderNotification(orderModel.OrderNumber);
            tasks.Add(sendEmail);

            await Task.WhenAll(tasks);

            return orderModel.OrderNumber;
        }
    }
}
