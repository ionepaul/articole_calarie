using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using PagedList;

namespace ArticoleCalarie.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductLogic _iProductLogic;
        private ISizeChartLogic _iSizeChartLogic;
        private IColorLogic _iColorLogic;
        private IBrandLogic _iBrandLogic;

        public ProductController(IProductLogic iProductLogic, ISizeChartLogic iSizeChartLogic, IColorLogic iColorLogic, IBrandLogic iBrandLogic)
        {
            _iProductLogic = iProductLogic;
            _iSizeChartLogic = iSizeChartLogic;
            _iColorLogic = iColorLogic;
            _iBrandLogic = iBrandLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> List(int pageNumber, string productCode = "")
        {
            var productsForAdmin = await _iProductLogic.GetProductsForAdmin(pageNumber, productCode);

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

            var pagedListModel = new StaticPagedList<ProductListItemModel>(productsForAdmin.Products, pageNumber, pageSize, productsForAdmin.TotalCount);

            return View(pagedListModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetSizeCharts()
        {
            return Json(_iSizeChartLogic.GetAllSizeCharts(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetColors()
        {
            return Json(_iColorLogic.GetAllColors(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetBrands(string searchTerm)
        {
            var brands = _iBrandLogic.GetAllBrands(searchTerm);

            return Json(brands, JsonRequestBehavior.AllowGet);
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
        public string UploadImage(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var formattedFileName = fileName.Replace(" ", "").Replace("-", "").Replace("_", "").Replace("(", "").Replace(")", "");
                    var serverPath = Server.MapPath(ConfigurationManager.AppSettings["ProductsImagesFolder"]);
                    var path = Path.Combine(serverPath, formattedFileName);
                    file.SaveAs(path);

                    return formattedFileName;
                }
                catch (Exception ex)
                {

                }
            }

            return null;
        }

        [HttpPost]
        public void DeleteImage(string fileName)
        {
            var serverPath = Server.MapPath(ConfigurationManager.AppSettings["ProductsImagesFolder"]);
            var filePath = Path.Combine(serverPath, fileName);

            if (Directory.Exists(Path.GetDirectoryName(serverPath)) && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}