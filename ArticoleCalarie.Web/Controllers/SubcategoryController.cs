using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Web.Controllers
{
    public class SubcategoryController : Controller
    {
        private ISubcategoryLogic _iSubcategoryLogic;

        public SubcategoryController(ISubcategoryLogic iSubcategoryLogic)
        {
            _iSubcategoryLogic = iSubcategoryLogic;
        }

        [HttpGet]
        public JsonResult GetSubcategories(int categoryId, string searchTerm)
        {
            var subcategories = _iSubcategoryLogic.GetAllSubcategories(categoryId, searchTerm);

            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult GetHorseNavbarSubcategories()
        {
            var horseSubcategories = _iSubcategoryLogic.GetAllSubcategories((int)CategoryViewEnum.Cal);

            return PartialView("_NavbarHorseSubcategories", horseSubcategories);
        }
    }
}