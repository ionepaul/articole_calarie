using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    public class ImageController : Controller
    {
        private static Logger _logger;

        public ImageController()
        {
            _logger = LogManager.GetLogger("Image");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public string UploadImage(HttpPostedFileBase file)
        {
            _logger.Info("POST > Upload Image");

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
                    _logger.Error($"Failed to save image {file.FileName}. Exception: {ex.Message}");
                }
            }

            return null;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void DeleteImage(string fileName)
        {
            _logger.Info("POST > Delete Image");

            try
            {
                var serverPath = Server.MapPath(ConfigurationManager.AppSettings["ProductsImagesFolder"]);
                var filePath = Path.Combine(serverPath, fileName);

                if (Directory.Exists(Path.GetDirectoryName(serverPath)) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to delete image {fileName}. Exception: {ex.Message}");
            }
        }
    }
}