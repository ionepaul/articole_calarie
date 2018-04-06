using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Infrastructure;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class EmailConverter
    {
        public static Dictionary<string, string> ToOrderParametersDictionary(this Order order)
        {
            var orderParamDictionary = new Dictionary<string, string>
            {
                [EmailParametersEnum.DeliveryAddressName.ToString().ToLower()] = order.DeliveryAddress?.FullName ?? string.Empty,
                [EmailParametersEnum.DeliveryAddressLine1.ToString().ToLower()] = order.DeliveryAddress?.AddressLine1 ?? string.Empty,
                [EmailParametersEnum.DeliveryAddressLine2.ToString().ToLower()] = order.DeliveryAddress?.AddressLine2 ?? string.Empty,
                [EmailParametersEnum.DeliveryAddressCity.ToString().ToLower()] = order.DeliveryAddress?.City ?? string.Empty,
                [EmailParametersEnum.DeliveryAddressCounty.ToString().ToLower()] = order.DeliveryAddress?.County ?? string.Empty,
                [EmailParametersEnum.DeliveryAddressCountry.ToString().ToLower()] = order.DeliveryAddress?.Country ?? string.Empty,
                [EmailParametersEnum.ViewOrderLink.ToString().ToLower()] = "/order/userorderlist",
                [EmailParametersEnum.OrderNumber.ToString().ToLower()] = order.OrderNumber.ToString(),
                [EmailParametersEnum.OrderDate.ToString().ToLower()] = order.OrderRegistrationDate.ToString(),
                [EmailParametersEnum.TotalAmount.ToString().ToLower()] = order.TotalAmount.ToString("F")
            };

            return orderParamDictionary;
        }

        public static Dictionary<string ,string> ToOrderItemParametersDictionary(this OrderItem orderItem)
        {
            var orderItemParamDictionary = new Dictionary<string, string>
            {
                [EmailParametersEnum.ImageName.ToString().ToLower()] = orderItem.ImageName ?? string.Empty,
                [EmailParametersEnum.ProductName.ToString().ToLower()] = orderItem.ProductName ?? string.Empty,
                [EmailParametersEnum.ProductColor.ToString().ToLower()] = orderItem.Color ?? string.Empty,
                [EmailParametersEnum.ProductSize.ToString().ToLower()] = orderItem.Size ?? string.Empty,
                [EmailParametersEnum.Quantity.ToString().ToLower()] = orderItem.Quantity.ToString() ?? string.Empty,
                [EmailParametersEnum.Price.ToString().ToLower()] = orderItem.Price.ToString("F")
            };

            return orderItemParamDictionary;
        }

        public static Dictionary<string, string> ToProductParametersDictionary(this Product product)
        {
            var productUrl = product.Subcategory?.Category?.Name.ToUrlString() + "/" + product.Subcategory?.Id + "/" + product.Subcategory?.Name.ToUrlString() + "/" + product.ProductName.ToUrlString();

            var productParamDictionary = new Dictionary<string, string>
            {
                [EmailParametersEnum.ProductName.ToString().ToLower()] = product.ProductName,
                [EmailParametersEnum.ImageName.ToString().ToLower()] = product.Images.FirstOrDefault()?.FileName ?? string.Empty,
                [EmailParametersEnum.Price.ToString().ToLower()] = product.Price.ToString("F"),
                [EmailParametersEnum.ProductDescription.ToString().ToLower()] = product.Description,
                [EmailParametersEnum.ProductUrl.ToString().ToLower()] = productUrl
            };

            return productParamDictionary;
        }

        #region Private Methods

        private static string ToUrlString(this string s)
        {
            s = s.ToLower().Replace(" ", "-");

            return s;
        }

        #endregion
    }
}
