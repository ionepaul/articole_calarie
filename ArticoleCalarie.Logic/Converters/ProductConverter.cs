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
                SalePercentage = productViewModel.SalePercentage ?? 0,
                Size = productViewModel.Size,
                ColorIds = new List<int>(),
                Images = new List<Image>()
            };

            return product;
        }

        public static ProductListItemModel ToListItemModel(this Product product)
        {
            var productListItemModel = new ProductListItemModel
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                SubcategoryName = product.Subcategory.Name,
                Brand = product.Brand?.Name,
                Price = product.Price,
                SalePercentage = product.SalePercentage.ToString() + "%"
                   
            };

            return productListItemModel;
        }
    }
}
