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
                var imgModel = new Image
                {
                    Name = image
                };

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

            product.ProductCode = "a";

            _iProductRepository.Add(product);
        }
    }
}
