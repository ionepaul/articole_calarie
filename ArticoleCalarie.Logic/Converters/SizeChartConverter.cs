using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class SizeChartConverter
    {
        public static SizeChartViewModel ToViewModel(this SizeChart sizeChart)
        {
            var sizeChartViewModel = new SizeChartViewModel
            {
                Id = sizeChart.Id,
                FileName = sizeChart.FileName
            };

            return sizeChartViewModel;
        }

        public static SizeChart ToDbModel(this SizeChartViewModel sizeChartViewModel)
        {
            var sizeChart = new SizeChart
            {
                Id = sizeChartViewModel.Id,
                FileName = sizeChartViewModel.FileName
            };

            return sizeChart;
        }
    }
}
