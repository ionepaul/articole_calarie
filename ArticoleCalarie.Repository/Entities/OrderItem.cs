using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticoleCalarie.Repository.Entities
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ImageName { get; set; }

        public decimal Price { get; set; }

        public int SalePercentage { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        [Required]
        public string ProductCode { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
