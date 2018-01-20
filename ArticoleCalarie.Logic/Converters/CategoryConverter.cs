using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.BLL.Converters
{
    public static class CategoryConverter
    {
        public static Category ToDbModel(this CategoryViewModel categoryViewModel)
        {
            var category = new Category
            {
                Name = categoryViewModel.Name
            };

            return category;
        }

        public static CategoryViewModel ToViewModel(this Category category)
        {
            var categoryViewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return categoryViewModel;
        }
    }
}
