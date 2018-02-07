using System.Collections.Generic;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.Models
{
    public class ProductSearchResult
    {
        public int TotalCount { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
