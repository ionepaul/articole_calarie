using System;
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

        public ProductLogic(IProductRepository iProductRepository)
        {
            _iProductRepository = iProductRepository;
        }

        public void AddProduct(ProductViewModel productViewModel)
        {
            var product = productViewModel.ToDbProduct();

            var images = productViewModel.Images?.Split(',');

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

            product.ProductCode = "a";

            _iProductRepository.Add(product);
        }
    }
}
