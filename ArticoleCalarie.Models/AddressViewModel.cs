using System.ComponentModel.DataAnnotations;
using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Models
{
    public class AddressViewModel
    {
        [Required]
        [Display(Name = "Nume Complet")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Adresă")]
        public string AddressLine1 { get; set; }

        [Required]
        [Display(Name = "Detalii Adresă")]
        public string AddressLine2 { get; set; }

        [Required]
        [Display(Name = "Oraș")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Cod Poștal")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Județ")]
        public string County { get; set; }

        [Required]
        [Display(Name = "Țară")]
        public string Country { get; set; }

        public AddressTypeViewEnum AddressType { get; set; }

        public bool IsSameAsDelivery { get; set; }
    }
}
