using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductLogic _iProductLogic;

        public ProductController(IProductLogic iProductLogic)
        {
            _iProductLogic = iProductLogic;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
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
    }
}