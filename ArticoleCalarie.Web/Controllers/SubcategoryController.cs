using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;

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
        [Authorize(Roles = "Admin")]
        public JsonResult GetSubcategories(int categoryId, string searchTerm)
        {
            var subcategories = _iSubcategoryLogic.GetAllSubcategories(categoryId, searchTerm);

            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }
    }
}