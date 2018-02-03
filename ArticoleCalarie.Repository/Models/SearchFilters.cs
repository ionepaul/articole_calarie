using System.Collections.Generic;

namespace ArticoleCalarie.Repository.Models
{
    public class SearchFilters
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public IEnumerable<ColorDTO> Colors { get; set; }
        public IEnumerable<string> Sizes { get; set; }
    }
}
