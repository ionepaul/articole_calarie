using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;

namespace ArticoleCalarie.Web.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryLogic _iCategoryLogic;

        public CategoryController(ICategoryLogic iCategoryLogic)
        {
            _iCategoryLogic = iCategoryLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetCategories()
        {
            var categories = _iCategoryLogic.GetAllCategories();

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
    }
}