using System.Collections.Generic;

namespace ArticoleCalarie.Repository.Models
{
    public class SearchModel
    {
        public int SubcategoryId { get; set; }
        public int ItemsToSkip { get; set; }
        public int ItemsPerPage { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<int> ColorIds { get; set; }
        public List<string> Sizes { get; set; }
    }
}
