using System.Web.Mvc;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            _logger.Info("Index");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}