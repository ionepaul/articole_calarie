using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [Display(Name = "Nume Complet")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Email-ul este invalid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [StringLength(100, ErrorMessage = "{0} trebuie sa aibă cel puțin {2} caractere.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă Parola")]
        [Compare("Password", ErrorMessage = "Parola și confirmarea parolei nu se potrivesc.")]
        public string ConfirmPassword { get; set; }
    }
}
