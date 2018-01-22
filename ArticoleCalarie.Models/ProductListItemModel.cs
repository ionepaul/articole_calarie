using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ProductListItemModel
    {
        [Display(Name = "Cod produs")]
        public string ProductCode { get; set; }

        [Display(Name = "Titlu produs")]
        public string ProductName { get; set; }

        [Display(Name = "Subcategorie")]
        public string SubcategoryName { get; set; }

        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Preț")]
        public decimal Price { get; set; }

        [Display(Name = "Reducere")]
        public string SalePercentage { get; set; }
    }
}
