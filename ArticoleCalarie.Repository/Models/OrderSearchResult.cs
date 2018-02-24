using System.Collections.Generic;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.Models
{
    public class OrderSearchResult
    {
        public int TotalCount { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
