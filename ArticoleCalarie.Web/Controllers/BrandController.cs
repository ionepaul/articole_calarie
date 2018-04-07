using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;

namespace ArticoleCalarie.Web.Controllers
{
    public class BrandController : Controller
    {
        private IBrandLogic _iBrandLogic;

        public BrandController(IBrandLogic iBrandLogic)
        {
            _iBrandLogic = iBrandLogic;
        }

        [HttpGet]
        public JsonResult GetBrands(string searchTerm)
        {
            var brands = _iBrandLogic.GetAllBrands(searchTerm);

            return Json(brands, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult GetNavbarBrands()
        {
            var brands = _iBrandLogic.GetAllBrands();

            return PartialView("_NavbarBrands", brands);
        }
    }
}