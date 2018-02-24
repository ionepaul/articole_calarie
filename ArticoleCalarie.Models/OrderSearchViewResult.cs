using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class OrderSearchViewResult
    {
        public int TotalCount { get; set; }

        public IEnumerable<OrderSummaryModel> Orders { get; set; }
    }
}
