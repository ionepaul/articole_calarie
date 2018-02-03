using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;

namespace ArticoleCalarie.Web.Controllers
{
    public class ColorController : Controller
    {
        private IColorLogic _iColorLogic;
        
        public ColorController(IColorLogic iColorLogic)
        {
            _iColorLogic = iColorLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetColors()
        {
            var colors = _iColorLogic.GetAllColors();

            return Json(colors, JsonRequestBehavior.AllowGet);
        }
    }
}