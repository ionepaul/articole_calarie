using System.Collections.Generic;
using System.Threading.Tasks;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Constants;
using ArticoleCalarie.Repository.IRepository;

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

        public async Task<int> PlaceOrder(CheckoutViewModel checkoutViewModel, string userId)
        {
            var orderModel = checkoutViewModel.ToDbModel();

            if (!string.IsNullOrEmpty(userId))
            {
                orderModel.UserId = userId;

                var user = await _iAccountRepository.FindUserByIdAsync(userId);

                if (user == null)
                {
                    orderModel.DeliveryAddress = checkoutViewModel.DeliveryAddress.ToDbAddress();
                    orderModel.BillingAddress = checkoutViewModel.BillingAddress.ToDbAddress();
                }
                else
                {
                    orderModel.DeliveryAddressId = user.DeliveryAddressId.HasValue ? user.DeliveryAddressId.Value : -1;
                    orderModel.BillingAddressId = user.BillingAddressId.HasValue ? user.BillingAddressId.Value : -1;
                    orderModel.Email = user.Email;
                }
            }
            else
            {
                orderModel.DeliveryAddress = checkoutViewModel.DeliveryAddress.ToDbAddress();
                orderModel.BillingAddress = checkoutViewModel.BillingAddress.ToDbAddress();
            }

            var totalOrders = await _iOrderRepository.CountOrders();

            orderModel.OrderNumber = ApplicationConstants.StartingOrderNumber + totalOrders;

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
