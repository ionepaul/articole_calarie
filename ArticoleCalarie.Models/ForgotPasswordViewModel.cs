using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Introduceti email-ul.")]
        [EmailAddress(ErrorMessage = "Email-ul introdus nu este valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
