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

        public ActionResult GetCategories(string term)
        {
            var categories = _iCategoryLogic.GetAllCategories(term);

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
    }
}