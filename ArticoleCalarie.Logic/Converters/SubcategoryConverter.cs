using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class SubcategoryConverter
    {
        public static SubcategoryViewModel ToViewModel(this Subcategory subcategory)
        {
            var subcategoryViewModel = new SubcategoryViewModel
            {
                Id = subcategory.Id,
                Name = subcategory.Name
            };

            return subcategoryViewModel;
        }

        public static Subcategory ToViewModel(this SubcategoryViewModel subcategoryViewModel)
        {
            var subcategory = new Subcategory
            {
                Id = subcategoryViewModel.Id,
                Name = subcategoryViewModel.Name
            };

            return subcategory;
        }
    }
}
