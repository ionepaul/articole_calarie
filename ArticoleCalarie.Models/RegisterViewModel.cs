using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nume Complet")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
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
