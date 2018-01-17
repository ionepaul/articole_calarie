using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.BLL.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class CategoryLogic : ICategoryLogic
    {
        private ICategoryRepository _iCategoryRepository;

        public CategoryLogic(ICategoryRepository iCategoryRepository)
        {
            _iCategoryRepository = iCategoryRepository;
        }

        public IEnumerable<CategoryViewModel> GetAllCategories(string term = "")
        {
            if (string.IsNullOrEmpty(term))
            {
                var categoriesViewModel = _iCategoryRepository.GetAll().Select(x => x.ToCategoryViewModel());

                return categoriesViewModel;
            }

            var categoriesViewModelByTerm = _iCategoryRepository.GetCategories(term).Select(x => x.ToCategoryViewModel());

            return categoriesViewModelByTerm;
        }
    }
}
