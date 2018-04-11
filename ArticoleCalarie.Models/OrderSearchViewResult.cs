using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class OrderSearchViewResult
    {
        public int TotalCount { get; set; }

        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
