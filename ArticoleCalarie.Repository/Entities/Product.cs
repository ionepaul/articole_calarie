using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticoleCalarie.Repository.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Index]
        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string MaterialDetails { get; set; }

        public int SalePercentage { get; set; }

        [Required]
        public int SubcategoryId { get; set; }

        public int? BrandId { get; set; }

        public int? SizeChartId { get; set; }

        [ForeignKey("SizeChartId")]
        public virtual SizeChart SizeChart { get; set; }

        [ForeignKey("SubcategoryId")]
        public virtual Subcategory Subcategory { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Color> AvailableColors { get; set; }

        [NotMapped]
        public List<int> ColorIds { get; set; }
    }
}
