using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ArticoleCalarie.Repository.Enums;

namespace ArticoleCalarie.Repository.Entities
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        public string PostalCode { get; set; }

        [Required]
        public string County { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public AddressType AddressType { get; set; }
    }
}
