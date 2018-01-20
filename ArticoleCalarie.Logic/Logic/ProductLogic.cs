using System;
using System.Collections.Generic;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class ProductLogic : IProductLogic
    {
        private IProductRepository _iProductRepository;
        private IColorRepository _iColorRepository;

        public ProductLogic(IProductRepository iProductRepository, IColorRepository iColorRepoository)
        {
            _iProductRepository = iProductRepository;
            _iColorRepository = iColorRepoository;
        }

        public void AddProduct(ProductViewModel productViewModel)
        {
            var product = productViewModel.ToDbProduct();

            ConvertPrice(product, productViewModel);
            StoreImages(product, productViewModel);
            StoreCategory(product, productViewModel);
            StoreBrand(product, productViewModel);

            if (!string.IsNullOrEmpty(productViewModel.SizeChartImage))
            {
                StoreSizeChart(product, productViewModel);
            }

            if (!string.IsNullOrEmpty(productViewModel.Colors))
            {
                StoreColors(product, productViewModel);
            }

            product.ProductCode = "a";

            _iProductRepository.Add(product);

            var productId = product.Id;
        }

        #region Private Methods

        private void StoreImages(Product product, ProductViewModel productViewModel)
        {
            var images = productViewModel.Images?.Split(',');

            foreach (var image in images)
            {
                var imgModel = new Image { FileName = image };

                product.Images.Add(imgModel);
            }
        }

        private void ConvertPrice(Product product, ProductViewModel productViewModel)
        {
            try
            {
                var price = Convert.ToDecimal(productViewModel.Price);

                product.Price = price;
            }
            catch (Exception)
            {
                //log and handle
            }
        }

        private void StoreCategory(Product product, ProductViewModel productViewModel)
        {
            try
            {
                var categoryId = Convert.ToInt32(productViewModel.CategoryId);

                product.CategoryId = categoryId;
            }
            catch (FormatException)
            {
                var newCategory = new Category { Name = productViewModel.CategoryId };

                product.Category = newCategory;
            }
        }

        private void StoreBrand(Product product, ProductViewModel productViewModel)
        {
            try
            {
                var brandId = Convert.ToInt32(productViewModel.Brand);

                product.BrandId = brandId;
            }
            catch (FormatException)
            {
                var newBrand = new Brand { Name = productViewModel.Brand };

                product.Brand = newBrand;
            }
        }

        private void StoreSizeChart(Product product, ProductViewModel productViewModel)
        {
            try
            {
                var sizeChartImageId = Convert.ToInt32(productViewModel.SizeChartImage);

                product.SizeChartId = sizeChartImageId;
            }
            catch (FormatException)
            {
                var newSizeChartImage = new SizeChart { FileName = productViewModel.SizeChartImage };

                product.SizeChart = newSizeChartImage;
            }
        }

        private void StoreColors(Product product, ProductViewModel productViewModel)
        {
            product.AvailableColors = new List<Color>();

            var availableColors = productViewModel.Colors?.Split(',');

            try
            {
                foreach (var colorId in availableColors)
                {
                    var intColorId = Convert.ToInt32(colorId);

                    var color = _iColorRepository.GetById(intColorId);

                    product.AvailableColors.Add(color);
                }
            }
            catch (Exception)
            {
                //log and handle
            }
        }

        #endregion
    }
}