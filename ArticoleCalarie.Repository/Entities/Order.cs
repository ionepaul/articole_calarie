using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticoleCalarie.Repository.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Index]
        public int OrderNumber { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        public int DeliveryAddressId { get; set; }

        public int BillingAddressId { get; set; }

        public decimal TotalAmount { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }

        [ForeignKey("DeliveryAddressId")]
        public virtual Address DeliveryAddress { get; set; }

        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
