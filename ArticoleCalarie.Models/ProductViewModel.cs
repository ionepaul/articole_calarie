using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Titlu produs")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Descriere")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Categorie")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Subcategorie")]
        public string SubcategoryId { get; set; }

        [Required]
        [Display(Name = "Preț")]
        public string Price { get; set; }

        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Detalii material")]
        public string MaterialDetails { get; set; }

        [Display(Name = "Imagini")]
        public string Images { get; set; }

        [Display(Name = "Culori")]
        public string Colors { get; set; }

        [Display(Name = "Tabel de marimi")]
        public string SizeChartImage { get; set; }

        [Display(Name = "Reducere (%)")]
        public int? SalePercentage { get; set; }

        [Display(Name = "Mărimi")]
        public string Size { get; set; }

        [Display(Name = "Cod produs")]
        public string ProductCode { get; set; }


        public List<string> ImagesList { get; set; }
        public List<string> ColorsList { get; set; }
    }
}
