using System.Linq;
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
                Name = subcategory.Name.Trim(),
                CategoryName = subcategory.Category?.Name.Trim()
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

        public static SubcategorySitemapModel ToSitemapModel(this Subcategory subcategory)
        {
            var subcategorySitemapModel = new SubcategorySitemapModel
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                CategoryName = subcategory.Category?.Name.Trim(),
                Products = subcategory.Products?.Select(x => x.ToMinimalInformationModel())
            };

            return subcategorySitemapModel;
        }
    }
}
