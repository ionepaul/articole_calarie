using System.Collections.Generic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface ISizeChartLogic
    {
        IEnumerable<SizeChartViewModel> GetAllSizeCharts();
    }
}
