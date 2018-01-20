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

            var images = productViewModel.Images?.Split(',');

            try
            {
                var price = Convert.ToDecimal(productViewModel.Price);

                product.Price = price;
            }
            catch (Exception)
            {
                //log and handle
            }

            foreach (var image in images)
            {
                var imgModel = new Image { FileName = image };

                product.Images.Add(imgModel);
            }

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

            if (!string.IsNullOrEmpty(productViewModel.SizeChartImage))
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

            if (!string.IsNullOrEmpty(productViewModel.Colors))
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

            product.ProductCode = "a";

            _iProductRepository.Add(product);
        }
    }
}