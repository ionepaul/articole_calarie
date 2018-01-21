using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;

namespace ArticoleCalarie.Web.Controllers
{
    public class SubcateogryController : Controller
    {
        private ISubcategoryLogic _iSubcategoryLogic;

        public SubcateogryController(ISubcategoryLogic iSubcategoryLogic)
        {
            _iSubcategoryLogic = iSubcategoryLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetSubategories(int categoryId, string searchTerm)
        {
            var subcategories = _iSubcategoryLogic.GetAllSubcategories(categoryId, searchTerm);

            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }
    }
}