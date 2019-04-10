using System.ComponentModel.DataAnnotations;
using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Models
{
    public class AddressViewModel
    {
        public AddressViewModel()
        {
            Country = "Romania";
        }

        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [Display(Name = "Nume Complet")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Numarul de telefon este obligatoriu.")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Adresa este obligatorie.")]
        [Display(Name = "Adresă")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Detalii Adresă")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Orasul este obligatoriu.")]
        [Display(Name = "Oraș")]
        public string City { get; set; }

        [Display(Name = "Cod Poștal")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Judetul este obligatoriu.")]
        [Display(Name = "Județ")]
        public string County { get; set; }

        [Required(ErrorMessage = "Tara este obligatorie.")]
        [Display(Name = "Țară")]
        public string Country { get; set; }

        public AddressTypeViewEnum AddressType { get; set; }

        public bool IsSameAsDelivery { get; set; }
    }
}
