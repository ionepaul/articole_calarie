using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Models;

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

        public static ProductViewModel ToViewModel(this Product product)
        {
            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                ProductCode = product.ProductCode,
                MaterialDetails = product.MaterialDetails,
                Size = product.Size,
                Price = product.Price.ToString("F"),
                SizeChartImage = product.SizeChart?.FileName,
                CategoryId = product.Subcategory?.CategoryId ?? 0,
                Brand = product.Brand?.Name,
                SubcategoryId = product.Subcategory?.Name,
                ImagesList = product.Images.Select(x => x.FileName).ToList(),
                ColorsList = product.AvailableColors.Select(x => x.Name).ToList(),
                IsOnSale = product.SalePercentage != 0
            };

            if (productViewModel.IsOnSale)
            {
                var saleValue = product.SalePercentage > 0 ? product.SalePercentage : (-1) * product.SalePercentage;

                productViewModel.SalePercentage = saleValue;

                var salePercentage = (decimal)saleValue / 100;

                var saleAmount = product.Price * salePercentage;

                productViewModel.PriceAfterSaleApplied = Math.Round(product.Price - saleAmount, 2);
            }

            return productViewModel;
        }

        public static ProductListItemModel ToListItemModel(this Product product)
        {
            var productListItemModel = new ProductListItemModel
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                SubcategoryName = product.Subcategory.Name,
                Brand = product.Brand?.Name,
                Price = product.Price,
                SalePercentage = product.SalePercentage.ToString() + "%"
                   
            };

            return productListItemModel;
        }

        public static ProductListViewItemModel ToListViewItemModel(this Product product)
        {
            var daysToKeepProductMarkedNew = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToKeepProductMarkedNew"]);

            var productListViewItemModel = new ProductListViewItemModel
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                ProductImageName = product.Images?.FirstOrDefault()?.FileName,
                IsNew = product.DatePosted > DateTime.Now.AddDays((-1) * daysToKeepProductMarkedNew),
                IsOnSale = product.SalePercentage != 0
            };

            if (productListViewItemModel.IsOnSale)
            {
                var saleValue = product.SalePercentage > 0 ? product.SalePercentage : (-1) * product.SalePercentage;

                productListViewItemModel.SalePercentage = saleValue;

                var salePercentage =(decimal)saleValue / 100;

                var saleAmount = productListViewItemModel.Price * salePercentage;

                productListViewItemModel.PriceAfterSaleApplied = Math.Round(productListViewItemModel.Price - saleAmount, 2);
            }

            return productListViewItemModel;
        }

        public static SearchViewFilters ToViewModel(this SearchFilters searchFilters)
        {
            var searchViewFilters = new SearchViewFilters
            {
                MinPrice = searchFilters.MinPrice,
                MaxPrice = searchFilters.MaxPrice,
                Colors = searchFilters.Colors.Select(x => x.ToViewModel()).ToList(),
                Sizes = searchFilters.Sizes.ToList()
            };

            return searchViewFilters;
        }
    }
}
