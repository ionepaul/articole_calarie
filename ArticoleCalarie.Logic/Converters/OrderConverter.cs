using System.Linq;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Enums;

namespace ArticoleCalarie.Logic.Converters
{
    public static class OrderConverter
    {
        public static Order ToDbModel(this OrderViewModel orderViewModel)
        {
            var order = new Order
            {
                Email = orderViewModel.Email,
                TotalAmount = orderViewModel.TotalAmount,
                OrderItems = orderViewModel.ShoppingItems.Select(x => x.ToDbModel()).ToList(),
                OrderStatus = OrderStatus.REGISTRED
            };

            return order;
        }

        public static OrderItem ToDbModel(this ShoppingCartItem shoppingItem)
        {
            var orderItem = new OrderItem
            {
                ProductCode = shoppingItem.ProductCode,
                ProductName = shoppingItem.ProductName,
                Price = shoppingItem.Price,
                SalePercentage = shoppingItem.SalePercentage,
                Quantity = shoppingItem.Quantity,
                Size = shoppingItem.Size,
                Color = shoppingItem.Color,
                ImageName = shoppingItem.ImageName
            };

            return orderItem;
        }
        
        public static ShoppingCartItem ToShoppingCartItem(this OrderItem orderItem)
        {
            var shoppingCartItem = new ShoppingCartItem
            {
                ProductCode = orderItem.ProductCode,
                ProductName = orderItem.ProductName,
                Price = orderItem.Price,
                SalePercentage = orderItem.SalePercentage,
                Quantity = orderItem.Quantity,
                Size = orderItem.Size,
                Color = orderItem.Color,
                ImageName = orderItem.ImageName
            };

            return shoppingCartItem;
        }

        public static OrderViewModel ToViewModel(this Order order)
        {
            var orderViewModel = new OrderViewModel
            {
                OrderNumber = order.OrderNumber,
                Email = order.Email,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToViewEnum(),
                DeliveryAddress = order.DeliveryAddress.ToViewModel(),
                BillingAddress = order.BillingAddress.ToViewModel(),
                ShoppingItems = order.OrderItems.Select(x => x.ToShoppingCartItem()).ToList(),
                OrderRegistrationDate = order.OrderRegistrationDate
            };

            return orderViewModel;
        }

        public static OrderStatusViewEnum ToViewEnum(this OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.REGISTRED:
                    return OrderStatusViewEnum.REGISTRED;
                case OrderStatus.CONFIRMED:
                    return OrderStatusViewEnum.CONFIRMED;
                case OrderStatus.SHIPPED:
                    return OrderStatusViewEnum.SHIPPED;
                case OrderStatus.COMPLETE:
                    return OrderStatusViewEnum.COMPLETE;
                default:
                    return OrderStatusViewEnum.UNIDENTIFIED;
            }
        }

        public static OrderStatus ToDbEnum(this OrderStatusViewEnum orderStatusViewEnum)
        {
            switch (orderStatusViewEnum)
            {
                case OrderStatusViewEnum.REGISTRED:
                    return OrderStatus.REGISTRED;
                case OrderStatusViewEnum.CONFIRMED:
                    return OrderStatus.CONFIRMED;
                case OrderStatusViewEnum.SHIPPED:
                    return OrderStatus.SHIPPED;
                case OrderStatusViewEnum.COMPLETE:
                    return OrderStatus.COMPLETE;
                default:
                    return OrderStatus.UNIDENTIFIED;
            }
        }
    }
}
