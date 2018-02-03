﻿using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class SubcategoryLogic : ISubcategoryLogic
    {
        private ISubcategoryRepository _iSubcategoryRepository;

        public SubcategoryLogic(ISubcategoryRepository iSubcategoryRepository)
        {
            _iSubcategoryRepository = iSubcategoryRepository;
        }

        public IEnumerable<SubcategoryViewModel> GetAllSubcategories(int categoryId, string searchTerm = "")
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var subcategories = _iSubcategoryRepository.GetAllByCategoryId(categoryId).Select(x => x.ToViewModel());

                return subcategories;
            }

            var subcategoriesByTerm = _iSubcategoryRepository.GetAllByCategoryIdAndSearchTerm(categoryId, searchTerm)
                                                             .Select(x => x.ToViewModel());

            return subcategoriesByTerm;
        }
    }
}
