using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class GuestEmailViewModel
    {
        [Required(ErrorMessage = "Introduceți adresa de email.")]
        public string Email { get; set; }
    }
}
