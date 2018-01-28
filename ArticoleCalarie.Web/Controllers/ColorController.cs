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
            return Json(_iColorLogic.GetAllColors(), JsonRequestBehavior.AllowGet);
        }
    }
}