using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class BrandConverter
    {
        public static BrandViewModel ToViewModel(this Brand brand)
        {
            var brandViewModel = new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name
            };

            return brandViewModel;
        }

        public static Brand ToDbModel(this BrandViewModel brandViewModel)
        {
            var brand = new Brand
            {
                Id = brandViewModel.Id,
                Name = brandViewModel.Name
            };

            return brand;
        }
    }
}
