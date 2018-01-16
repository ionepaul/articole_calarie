using System.ComponentModel.DataAnnotations;
using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Models
{
    public class AddressViewModel
    {
        [Display(Name = "Nume Complet")]
        public string FullName { get; set; }

        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Adresă")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Detalii Adresă")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Oraș")]
        public string City { get; set; }

        [Display(Name = "Cod Poștal")]
        public string PostalCode { get; set; }

        [Display(Name = "Județ")]
        public string County { get; set; }

        [Display(Name = "Țară")]
        public string Country { get; set; }

        public AddressTypeViewEnum AddressType { get; set; }
    }
}
