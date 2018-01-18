using System;
using System.Collections.Generic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class ProductConverter
    {
        public static Product ToDbProduct(this ProductViewModel productViewModel)
        {
            var product = new Product
            {
                ProductName = productViewModel.ProductName,
                Description = productViewModel.Description,
                DatePosted = DateTime.UtcNow,
                MaterialDetails = productViewModel.MaterialDetails,
                Price = productViewModel.Price,
                Brand = productViewModel.Brand,
                Images = new List<Image>()
            };

            return product;
        }
    }
}
