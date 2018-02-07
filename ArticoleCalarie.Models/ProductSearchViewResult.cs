using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class ProductSearchViewResult
    {
        public int TotalCount { get; set; }

        public IEnumerable<ProductListViewItemModel> Products { get; set; }
    
        public virtual object PagedProductModel { get; set; }
    }
}
