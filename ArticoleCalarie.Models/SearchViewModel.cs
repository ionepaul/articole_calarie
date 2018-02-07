namespace ArticoleCalarie.Models
{
    public class SearchViewModel
    {
        public int SubcategoryId { get; set; }
        public int PageNumber { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string ColorIds { get; set; }
        public string Sizes { get; set; }
    }
}
