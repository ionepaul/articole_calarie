using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using Newtonsoft.Json;
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

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["AdminProductsPerPage"]);

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

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["AdminProductsPerPage"]);

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
        [Route("produse/{categoryName}/{subcategoryId}/{subcategoryName}", Name = "product-list-url")]
        public async Task<ActionResult> ProductViewList(string categoryName, int subcategoryId, string subcategoryName, int page, decimal? minp, decimal? maxp, string cl = "", string sz = "")
        {
            _logger.Info("VIEW > Product List By Subcategory and Search Model");

            ViewBag.SubcategoryId = subcategoryId;
            ViewBag.CategoryName = categoryName;
            ViewBag.SubcategoryName = subcategoryName;

            var searchViewModel = new SearchViewModel();
            var sessionSearchModel = Session["SearchModel"] as SearchViewModel;

            try
            {
                if ((sessionSearchModel != null && sessionSearchModel.SubcategoryId == subcategoryId && minp == null && maxp == null && string.IsNullOrEmpty(cl) && string.IsNullOrEmpty(sz)))
                {
                    searchViewModel = sessionSearchModel;
                    searchViewModel.PageNumber = page;
                }
                else
                {
                    searchViewModel = new SearchViewModel
                    {
                        SubcategoryId = subcategoryId,
                        PageNumber = page,
                        MinPrice = minp,
                        MaxPrice = maxp,
                        ColorIds = cl,
                        Sizes = sz
                    };

                    Session["SearchModel"] = searchViewModel;
                }

                var productSearchViewResult = await _iProductLogic.GetProductsBySearch(searchViewModel);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

                var pagedListModel = new StaticPagedList<ProductListViewItemModel>(productSearchViewResult.Products, page, pageSize, productSearchViewResult.TotalCount);

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get products by search for subcategory {subcategoryId}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        [Route("produse/{categoryName}/{subcategoryId}/{subcategoryName}/{productCode}/{productName}", Name = "product-details-url")]
        public ActionResult Details(string categoryName, int subcategoryId, string subcategoryName, string productCode, string productName)
        {
            _logger.Info("VIEW > Product Detail");

            ViewBag.CategoryName = categoryName;
            ViewBag.SubcategoryName = subcategoryName;
            ViewBag.SubcategoryId = subcategoryId;

            try
            {
                var product = _iProductLogic.GetProductByProductCode(productCode);

                var _subcategoryCookie = new HttpCookie("_RelatedProductsIn")
                {
                    Value = product.SubcategoryId,
                    Expires = DateTime.Now.AddDays(10)
                };

                Response.Cookies.Add(_subcategoryCookie);

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get product by product code: {productCode}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRelatedProducts()
        {
            _logger.Info("PARTIAL VIEW > Related Products");

            try
            {
                var subcategory = Request.Cookies["_RelatedProductsIn"]?.Value;

                var relatedProducts = await _iProductLogic.GetRelatedProducts(subcategory);

                return PartialView("_RelatedProductsPartial", relatedProducts);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to retrive related products. Exception: {ex.Message}");

                return PartialView("_RelatedProductsPartial", new List<ProductListViewItemModel>());
            }
        }

        [HttpGet]
        [Route("produse/noutati", Name = "newest-products-url")]
        public async Task<ActionResult> NewestProducts(int? pageNumber, bool isForHomePage = false)
        {
            if (isForHomePage)
            {
                _logger.Info("PARTIAL VIEW > The newest four products for home page.");

                try
                {
                    var newestProductsForHome = await _iProductLogic.GetTheNewestProductsForHome();

                    return PartialView("_RelatedProductsPartial", newestProductsForHome);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to get the newest four products for home page. Exception: {ex.Message}");

                    return PartialView("_RelatedProductsPartial", new List<ProductListViewItemModel>());
                }
            }

            try
            {
                _logger.Info("VIEW > The newest products page.");

                var page = pageNumber ?? 1;

                var products = await _iProductLogic.GetTheNewestPoducts(page);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsOnSaleAndNewestPerPage"]);

                var pagedListModel = new StaticPagedList<ProductListViewItemModel>(products.Products, page, pageSize, products.TotalCount);

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get products on sale. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult ProductListSearchPartial(int subcategoryId)
        {
            var searchFilters = _iProductLogic.GetSearchViewFiltersForSubcategory(subcategoryId);

            return PartialView("_ProductListSearch", searchFilters);
        }

        [HttpGet]
        [Route("produse/brand/{brand}", Name = "products-by-brand-url")]
        public async Task<ActionResult> ProductsByBrand(string brand, int? pageNumber)
        {
            _logger.Info("VIEW > Products by brand id");

            ViewBag.BrandName = brand;

            try
            {
                var page = pageNumber ?? 1;

                var products = await _iProductLogic.GetProductsByBrand(brand, page);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsOnSaleAndNewestPerPage"]);

                var pagedListModel = new StaticPagedList<ProductListViewItemModel>(products.Products, page, pageSize, products.TotalCount);

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get products by brand {brand}. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        [Route("produse/oferte", Name = "products-on-sale-url")]
        public async Task<ActionResult> ProductsOnSale(int? pageNumber, bool isForHomePage = false)
        {
            if (!isForHomePage)
            {
                _logger.Info("VIEW > Products on sale");

                try
                {
                    var page = pageNumber ?? 1;

                    var products = await _iProductLogic.GetProductsOnSale(page);

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsOnSaleAndNewestPerPage"]);

                    var pagedListModel = new StaticPagedList<ProductListViewItemModel>(products.Products, page, pageSize, products.TotalCount);

                    return View(pagedListModel);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to get products on sale. Exception: {ex.Message}.");

                    return View("Error");
                }
            } 

            _logger.Info("PARTIAL VIEW > The latest four products on sale products for home page.");

            try
            {
                var productsOnSaleForHome = await _iProductLogic.GetProductsOnSaleForHome();

                return PartialView("_RelatedProductsPartial", productsOnSaleForHome);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get the lates four products on sale for home page. Exception: {ex.Message}");

                return PartialView("_RelatedProductsPartial", new List<ProductListViewItemModel>());
            }
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
                _logger.Error($"Failed to add product {productViewModel.ProductName}. Object: {JsonConvert.SerializeObject(productViewModel)} --  Exception: {ex.Message}.");

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
                _logger.Error($"Failed to update product {productViewModel.ProductName}. Object: {JsonConvert.SerializeObject(productViewModel)} -- Exception: {ex.Message}.");

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