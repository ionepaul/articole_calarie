using System.Linq;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class OrderConverter
    {
        public static Order ToDbModel(this CheckoutViewModel checkoutViewModel)
        {
            var order = new Order
            {
                Email = checkoutViewModel.Email,
                TotalAmount = checkoutViewModel.TotalAmount,
                OrderItems = checkoutViewModel.ShoppingItems.Select(x => x.ToDbModel()).ToList()
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
    }
}
