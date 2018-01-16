using System.Threading.Tasks;
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

        public async Task AddCategory(CategoryViewModel categoryViewModel)
        {
            var category = categoryViewModel.ToDbCategory();

            await _iCategoryRepository.Add(category);
        }
    }
}
