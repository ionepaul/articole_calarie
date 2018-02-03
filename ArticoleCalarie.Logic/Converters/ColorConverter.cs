using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Models;

namespace ArticoleCalarie.Logic.Converters
{
    public static class ColorConverter
    {
        public static ColorViewModel ToViewModel(this Color color)
        {
            var colorViewModel = new ColorViewModel
            {
                Id = color.Id,
                Hex = color.HexValue,
                Name = color.Name
            };

            return colorViewModel;
        }

        public static Color ToDbModel(this ColorViewModel colorViewModel)
        {
            var color = new Color
            {
                Id = colorViewModel.Id,
                HexValue = colorViewModel.Hex,
                Name = colorViewModel.Name
            };

            return color;
        }

        public static ColorViewModel ToViewModel(this ColorDTO color)
        {
            var colorViewModel = new ColorViewModel
            {
                Id = color.Id,
                Hex = color.HexValue
            };

            return colorViewModel;
        }
    }
}
