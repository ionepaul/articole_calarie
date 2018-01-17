using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ProductViewModel
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Brand { get; set; }

        public string MaterialDetails { get; set; }

        public IEnumerable<object> Categories { get; set; }
    }
}
