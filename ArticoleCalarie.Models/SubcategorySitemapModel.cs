using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class SubcategorySitemapModel : SubcategoryViewModel
    {
        public IEnumerable<ProductMinimalInformation> Products { get; set; }
    }
}
