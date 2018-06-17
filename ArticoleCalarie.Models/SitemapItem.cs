using System;

namespace ArticoleCalarie.Models
{
    public class SitemapItem
    {
        public DateTime? DateAdded { get; set; }
        public string URL { get; set; }
        public string Priority { get; set; }
    }
}
