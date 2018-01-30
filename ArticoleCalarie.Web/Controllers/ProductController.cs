using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using PagedList;

namespace ArticoleCalarie.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductLogic _iProductLogic;

        public ProductController(IProductLogic iProductLogic)
        {
            _iProductLogic = iProductLogic;
        }

        #region HttpGet

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var product = _iProductLogic.GetProductById(id);

            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ViewResult List(int? pageNumber, string productCode = "")
        {
            ViewBag.ProductCode = productCode;

            var page = pageNumber ?? 1;

            var productsForAdmin = _iProductLogic.GetProductsForAdmin(page, productCode);

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

            var pagedListModel = new StaticPagedList<ProductListItemModel>(productsForAdmin.Products, page, pageSize, productsForAdmin.TotalCount);

            return View(pagedListModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public PartialViewResult ProductListForAdmin(int? pageNumber, string productCode = "")
        {
            ViewBag.ProductCode = productCode;

            var page = pageNumber ?? 1;

            var productsForAdmin = _iProductLogic.GetProductsForAdmin(page, productCode);

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

            var pagedListModel = new StaticPagedList<ProductListItemModel>(productsForAdmin.Products, page, pageSize, productsForAdmin.TotalCount);

            return PartialView("_ProductListForAdmin", pagedListModel);
        }

        [HttpGet]
        public async Task<ActionResult> ProductViewList(int subcategoryId, int pageNumber)
        {
            var searchViewModel = new SearchViewModel
            {
                SubcategoryId = subcategoryId,
                PageNumber = pageNumber
            };

            var productSearchViewResult = await _iProductLogic.GetProductsBySearch(searchViewModel);

            return View(productSearchViewResult);
        }

        #endregion

        #region HttpPost

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            //catch exceptions log
            _iProductLogic.AddProduct(productViewModel);

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            //catch exceptions log
            _iProductLogic.UpdateProduct(productViewModel.Id, productViewModel);

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int productId)
        {
            //catch exceptions log
            _iProductLogic.DeleteProduct(productId);

            return RedirectToAction(nameof(List));
        }

        #endregion
    }
}