using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class SizeChartLogic : ISizeChartLogic
    {
        private ISizeChartRepository _iSizeChartRepository;

        public SizeChartLogic(ISizeChartRepository iSizeChartRepository)
        {
            _iSizeChartRepository = iSizeChartRepository;
        }

        public IEnumerable<SizeChartViewModel> GetAllSizeCharts()
        {
            var sizeCharts = _iSizeChartRepository.GetAll();

            return sizeCharts.Select(x => x.ToViewModel());
        }
    }
}
