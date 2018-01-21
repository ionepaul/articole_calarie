using System;
using System.Collections.Generic;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Constants;
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
            StoreSubcategory(product, productViewModel);
            StoreBrand(product, productViewModel);

            if (!string.IsNullOrEmpty(productViewModel.SizeChartImage))
            {
                StoreSizeChart(product, productViewModel);
            }

            if (!string.IsNullOrEmpty(productViewModel.Colors))
            {
                StoreColors(product, productViewModel);
            }

            product.ProductCode = Guid.NewGuid().ToString();

            _iProductRepository.Add(product);

            var savedProduct = _iProductRepository.GetById(product.Id);

            var productCode = GenerateProductCode(savedProduct);

            _iProductRepository.UpdateProductCode(savedProduct.Id, productCode);
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

        private void StoreSubcategory(Product product, ProductViewModel productViewModel)
        {
            try
            {
                var subcategoryId = Convert.ToInt32(productViewModel.SubcategoryId);

                product.SubcategoryId = subcategoryId;
            }
            catch (FormatException)
            {
                var newSubcategory = new Subcategory { Name = productViewModel.SubcategoryId, CategoryId = productViewModel.CategoryId };

                product.Subcategory = newSubcategory;
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

            var selectedColors = productViewModel.Colors?.Split(',');

            try
            {
                foreach (var colorId in selectedColors)
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

        private string GenerateProductCode(Product product)
        {
            var categoryCode = string.Empty;

            switch (product.Subcategory.Category.Id)
            {
                case 1:
                    categoryCode = ApplicationConstants.HorseCategoryCode;
                    break;
                case 2:
                    categoryCode = ApplicationConstants.RiderCategoryCode;
                    break;
                case 3:
                    categoryCode = ApplicationConstants.StableCategoryCode;
                    break;
            }

            var subcategoryCode = product.Subcategory.Name.Substring(0, 2).ToUpperInvariant();
            var productIdCode = FormatProductId(product.Id);

            var productCode = categoryCode + subcategoryCode + "-" + productIdCode;

            return productCode;
        }

        private string FormatProductId(int productId)
        {
            var formmatedProductId = string.Empty;
            var numbers = new List<int>();

            while (productId != 0)
            {
                var cif = productId % 10;

                numbers.Add(cif);

                productId = productId / 10;
            }

            var arrayLength = numbers.Count;

            while (arrayLength != 0) {
                numbers.Add(0);
                arrayLength -= 1;
            }

            numbers.Reverse();

            foreach (var x in numbers)
            {
                formmatedProductId += x.ToString();
            }

            return formmatedProductId;
        }

        #endregion
    }
}