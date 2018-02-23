using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul produsului este obligatoriu.")]
        [Display(Name ="Titlu produs")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie.")]
        [Display(Name = "Descriere")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie.")]
        [Display(Name = "Categorie")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Subcategoria este obligatorie.")]
        [Display(Name = "Subcategorie")]
        public string SubcategoryId { get; set; }

        [Required(ErrorMessage = "Prețul este obligatoriu.")]
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

        public decimal PriceAfterSaleApplied { get; set; }

        public bool IsOnSale { get; set; }

        public List<string> ImagesList { get; set; }

        public List<string> ColorsList { get; set; }

        public List<ColorViewModel> ColorsViewModel { get; set; }
    }
}
