using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        private IImageRepository _iImageRepository;

        public ProductLogic(IProductRepository iProductRepository, IColorRepository iColorRepoository, IImageRepository iImageRepository)
        {
            _iProductRepository = iProductRepository;
            _iColorRepository = iColorRepoository;
            _iImageRepository = iImageRepository;
        }

        public void AddProduct(ProductViewModel productViewModel)
        {
            var product = productViewModel.ToDbProduct();

            ConvertPrice(product, productViewModel);
            StoreImages(product, productViewModel);
            StoreSubcategory(product, productViewModel);

            if (!string.IsNullOrEmpty(productViewModel.Brand))
            {
                StoreBrand(product, productViewModel);
            }

            if (!string.IsNullOrEmpty(productViewModel.SizeChartImage))
            {
                StoreSizeChart(product, productViewModel);
            }

            if (!string.IsNullOrEmpty(productViewModel.Colors))
            {
                StoreColors(product, productViewModel);
            }

            product.ProductCode = Guid.NewGuid().ToString();

            _iProductRepository.AddProductToDb(product);

            var savedProduct = _iProductRepository.GetProductById(product.Id);

            var productCode = GenerateProductCode(savedProduct);

            _iProductRepository.UpdateProductCode(savedProduct.Id, productCode);
        }

        public void UpdateProduct(int id, ProductViewModel productViewModel)
        {
            var product = _iProductRepository.GetProductById(id);

            product.ProductName = productViewModel.ProductName;
            product.Description = productViewModel.Description;
            product.MaterialDetails = productViewModel.MaterialDetails;
            product.Price = Convert.ToDecimal(productViewModel.Price);
            product.SalePercentage = productViewModel.SalePercentage ?? 0;
            product.Size = productViewModel.Size;

            if (!string.IsNullOrEmpty(productViewModel.SizeChartImage))
            {
                StoreSizeChart(product, productViewModel);
            }

            var images = productViewModel.Images?.Split(',');

            foreach (var savedImage in product.Images)
            {
                if (!images.Contains(savedImage.FileName))
                {
                    var img = _iImageRepository.GetById(savedImage.Id);

                    _iImageRepository.Delete(img);
                }
            }

            foreach (var image in images)
            {
                if (!product.Images.Select(x => x.FileName).Contains(image))
                {
                    var imgModel = new Image { FileName = image };

                    product.Images.Add(imgModel);
                }
            }

            var selectedColors = productViewModel.Colors?.Split(',');
            product.ColorIds = new List<int>();

            if (selectedColors != null)
            {
                foreach (var colorId in selectedColors)
                {
                    var intColorId = Convert.ToInt32(colorId);

                    product.ColorIds.Add(intColorId);
                }
            }

            if (!string.Equals(product.Subcategory?.Name, productViewModel.SubcategoryId))
            {
                StoreSubcategory(product, productViewModel);
            }
            
            if (!string.Equals(product.Brand?.Name, productViewModel.Brand))
            {
                StoreBrand(product, productViewModel);
            }

            _iProductRepository.UpdateProduct(product);
        }

        public void DeleteProduct(int productId)
        {
            var product = _iProductRepository.GetProductById(productId);

            var serverPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ProductsImagesFolder"]);

            foreach (var image in product.Images)
            {
                var filePath = Path.Combine(serverPath, image.FileName);

                if (Directory.Exists(Path.GetDirectoryName(serverPath)) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _iProductRepository.Delete(product);
        }

        public ProductListAdminViewModel GetProductsForAdmin(int pageNumber, string productCode)
        {
            var itemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);
            var itemsToSkip = (pageNumber - 1) * itemsPerPage;

            var productsForAdmin = _iProductRepository.GetProductsForAdmin(itemsPerPage, itemsToSkip, productCode);

            var productListAdminViewModel = new ProductListAdminViewModel
            {
                TotalCount = productsForAdmin.TotalCount,
                Products = productsForAdmin.Products.Select(x => x.ToListItemModel())
            };

            return productListAdminViewModel;
        }

        public ProductViewModel GetProductByProductCode(string productCode)
        {
            var product = _iProductRepository.GetProductByProductCode(productCode);

            var productViewModel = product.ToViewModel();

            productViewModel.ColorsViewModel = product.AvailableColors?.Select(x => x.ToViewModel()).ToList();

            return productViewModel;
        }

        public async Task<ProductSearchViewResult> GetProductsBySearch(SearchViewModel searchViewModel)
        {
            var searchModel = searchViewModel.ToDbSearchModel();

            var productSearchResult = await _iProductRepository.GetProductsBySearch(searchModel);

            var productSearchViewResult = new ProductSearchViewResult
            {
                TotalCount = productSearchResult.TotalCount,
                Products = productSearchResult.Products.Select(x => x.ToListViewItemModel())
            };

            return productSearchViewResult;
        }

        public ProductViewModel GetProductById(int id)
        {
            var product = _iProductRepository.GetProductById(id);

            if (product == null)
            {
                throw new Exception("Invalid product identifier.");
            }

            var productViewModel = product.ToViewModel();

            return productViewModel;
        }

        public SearchViewFilters GetSearchViewFiltersForSubcategory(int subcategoryId)
        {
            var searchFilters = _iProductRepository.GetSearchFiltersForSubcategory(subcategoryId);

            var searchViewFilters = searchFilters.ToViewModel();

            return searchViewFilters;
        }

        public async Task<ProductSearchViewResult> GetProductsByBrand(string brand, int pageNumber)
        {
            var itemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);
            var itemsToSkip = (pageNumber - 1) * itemsPerPage;

            var productsByBrand = await _iProductRepository.GetProductsByBrand(brand, itemsPerPage, itemsToSkip);

            var productSearchViewResult = new ProductSearchViewResult
            {
                TotalCount = productsByBrand.TotalCount,
                Products = productsByBrand.Products.Select(x => x.ToListViewItemModel())
            };

            return productSearchViewResult;
        }

        public async Task<ProductSearchViewResult> GetProductsOnSale(int pageNumber)
        {
            var itemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);
            var itemsToSkip = (pageNumber - 1) * itemsPerPage;

            var productsOnSale = await _iProductRepository.GetProductsOnSale(itemsPerPage, itemsToSkip);

            var productSearchViewResult = new ProductSearchViewResult
            {
                TotalCount = productsOnSale.TotalCount,
                Products = productsOnSale.Products.Select(x => x.ToListViewItemModel())
            };

            return productSearchViewResult;
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
                throw new Exception("Invalid price.");
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
                var newSubcategory = new Subcategory
                {
                    Name = productViewModel.SubcategoryId,
                    CategoryId = productViewModel.CategoryId
                };

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
            var selectedColors = productViewModel.Colors?.Split(',');

            try
            {
                foreach (var colorId in selectedColors)
                {
                    var intColorId = Convert.ToInt32(colorId);

                    product.ColorIds.Add(intColorId);
                }
            }
            catch (Exception)
            {
                throw new Exception("Invalid color identifier.");
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

            var zeroToAdd = 4 - numbers.Count;

            while (zeroToAdd != 0)
            {
                numbers.Add(0);
                zeroToAdd -= 1;
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