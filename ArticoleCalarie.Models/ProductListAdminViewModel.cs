using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class ProductListAdminViewModel
    {
        public int TotalCount { get; set; }

        public IEnumerable<ProductListItemModel> Products { get; set; }
    }
}
