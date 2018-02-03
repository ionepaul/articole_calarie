using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class SearchViewFilters
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set;  }
        public List<ColorViewModel> Colors { get; set; }
        public List<string> Sizes { get; set; }
    }
}
