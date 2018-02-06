using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using NLog;
using PagedList;

namespace ArticoleCalarie.Web.Controllers
{
    public class ProductController : Controller
    {
        private static Logger _logger;
        private IProductLogic _iProductLogic;

        public ProductController(IProductLogic iProductLogic)
        {
            _logger = LogManager.GetLogger("Product");
            _iProductLogic = iProductLogic;
        }

        #region HttpGet

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            _logger.Info("VIEW > Add Product");

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            _logger.Info("VIEW > Edit Product");

            try
            {
                var product = _iProductLogic.GetProductById(id);

                _logger.Info("Successfully retrieved product for edit.");

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get product. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ViewResult List(int? pageNumber, string productCode = "")
        {
            _logger.Info("VIEW > Admin Product List");
            
            ViewBag.ProductCode = productCode;

            try
            {
                var page = pageNumber ?? 1;

                var productsForAdmin = _iProductLogic.GetProductsForAdmin(page, productCode);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

                var pagedListModel = new StaticPagedList<ProductListItemModel>(productsForAdmin.Products, page, pageSize, productsForAdmin.TotalCount);

                _logger.Info("Successfully got product list for admin.");

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get product list for admin. Exception: {ex.Message}.");

                return View("Error");
            }            
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ProductListForAdmin(int? pageNumber, string productCode = "")
        {
            _logger.Info("PARTIAL VIEW > Admin Product List Search");

            ViewBag.ProductCode = productCode;

            try
            {
                var page = pageNumber ?? 1;

                var productsForAdmin = _iProductLogic.GetProductsForAdmin(page, productCode);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

                var pagedListModel = new StaticPagedList<ProductListItemModel>(productsForAdmin.Products, page, pageSize, productsForAdmin.TotalCount);

                return PartialView("_ProductListForAdmin", pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get product list by product code for admin. Search Term: {productCode}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ProductViewList(int subcategoryId, int pageNumber, decimal? minPrice, decimal? maxPrice, string colors = "", string sizes = "")
        {
            _logger.Info("VIEW > Product List By Subcategory and Search Model");

            ViewBag.SubcategoryId = subcategoryId;

            try
            {
                var searchViewModel = new SearchViewModel
                {
                    SubcategoryId = subcategoryId,
                    PageNumber = pageNumber,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    ColorIds = colors,
                    Sizes = sizes
                };

                var productSearchViewResult = await _iProductLogic.GetProductsBySearch(searchViewModel);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

                var pagedListModel = new StaticPagedList<ProductListViewItemModel>(productSearchViewResult.Products, pageNumber, pageSize, productSearchViewResult.TotalCount);

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get products by search for subcategory {subcategoryId}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Details(string productCode)
        {
            _logger.Info("VIEW > Product Detail for Admin");

            try
            {
                var product = _iProductLogic.GetProductByProductCode(productCode);

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get product by product code: {productCode} for admin. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult ProductListSearchPartial(int subcategoryId)
        {
            var searchFilters = _iProductLogic.GetSearchViewFiltersForSubcategory(subcategoryId);

            return PartialView("_ProductListSearch", searchFilters);
        }

        #endregion

        #region HttpPost

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(ProductViewModel productViewModel)
        {
            _logger.Info("POST > Add product");

            if (!ModelState.IsValid)
            {
                _logger.Info("Invalid ProductModel. Returning AddProduct view.");

                return View(productViewModel);
            }
            
            try
            {
                _iProductLogic.AddProduct(productViewModel);

                _logger.Info($"Successfully added product {productViewModel.ProductName}.");

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to add product {productViewModel.ProductName}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            _logger.Info("POST > Edit product");

            if (!ModelState.IsValid)
            {
                _logger.Info("Invalid ProductModel. Returning EditProduct view.");

                return View(productViewModel);
            }

            try
            {
                _iProductLogic.UpdateProduct(productViewModel.Id, productViewModel);

                _logger.Info($"Successfully updated product {productViewModel.ProductName}.");

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update product {productViewModel.ProductName}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int productId)
        {
            _logger.Info("POST > Delete Product");

            try
            {
                _iProductLogic.DeleteProduct(productId);

                _logger.Info($"Successfully deleted product with Id: {productId}.");

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to delete product with Id: {productId}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        #endregion
    }
}