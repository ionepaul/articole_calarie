﻿using System.Collections.Generic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface ISubcategoryLogic
    {
        IEnumerable<SubcategorySitemapModel> GetAllSubcategories();
        IEnumerable<SubcategoryViewModel> GetAllSubcategories(int categoryId, string searchTerm = "");
        IEnumerable<SubcategoryViewModel> GetAllSubcategoriesByCategoryName(string categoryName);
    }
}
