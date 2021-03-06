﻿using System.Web.Mvc;
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
        public PartialViewResult GetNavbarSubcategories(int categoryId)
        {
            var subcategories = _iSubcategoryLogic.GetAllSubcategories(categoryId);

            return PartialView("_NavbarSubcategories", subcategories);
        }

        [HttpGet]
        [Route("{categoryName}/subcategorii", Name = "subcategories-url")]
        public ActionResult SubcategoryList(string categoryName)
        {
            var subcategories = _iSubcategoryLogic.GetAllSubcategoriesByCategoryName(categoryName);

            return View(subcategories);
        }
    }
}