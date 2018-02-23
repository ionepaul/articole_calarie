using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class GuestEmailViewModel
    {
        [Required(ErrorMessage = "Introduceți adresa de email.")]
        [EmailAddress(ErrorMessage = "Adresa de email este invalidă.")]
        public string Email { get; set; }
    }
}
