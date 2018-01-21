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

        public IEnumerable<CategoryViewModel> GetAllCategories()
        {
            var categories = _iCategoryRepository.GetAll().Select(x => x.ToViewModel());

            return categories;
        }
    }
}
