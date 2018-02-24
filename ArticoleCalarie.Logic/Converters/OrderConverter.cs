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
                Color = shoppingItem.Color
            };

            return orderItem;
        }

        public static OrderSummaryModel ToSummaryModel(this Order order)
        {
            var orderSummaryModel = new OrderSummaryModel
            {
                OrderNumber = order.OrderNumber,
                Email = order.Email,
                OrderStatus = order.OrderStatus.ToViewEnum(),
                TotalAmount = order.TotalAmount,
                //Products = string.Join(", ", order.OrderItems.Select(x => x.ProductName))
            };

            return orderSummaryModel;
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
