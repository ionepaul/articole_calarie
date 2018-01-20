using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductLogic _iProductLogic;
        private ISizeChartLogic _iSizeChartLogic;
        private IColorLogic _iColorLogic;

        public ProductController(IProductLogic iProductLogic, ISizeChartLogic iSizeChartLogic, IColorLogic iColorLogic)
        {
            _iProductLogic = iProductLogic;
            _iSizeChartLogic = iSizeChartLogic;
            _iColorLogic = iColorLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public JsonResult GetSizeCharts()
        {
            return Json(_iSizeChartLogic.GetAllSizeCharts(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public JsonResult GetColors()
        {
            return Json(_iColorLogic.GetAllColors(), JsonRequestBehavior.AllowGet);
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

            return View();
        }

        [HttpPost]
        public string UploadImage(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var formattedFileName = fileName.Replace(" ", "").Replace("-", "").Replace("_", "");
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