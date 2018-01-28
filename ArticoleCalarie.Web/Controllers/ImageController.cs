using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ArticoleCalarie.Web.Controllers
{
    public class ImageController : Controller
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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